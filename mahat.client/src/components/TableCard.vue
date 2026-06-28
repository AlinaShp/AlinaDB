<template>
  <div class="tablecard-wrapper">
    <div class="tablecard">
      <!-- Delete table button -->
      <button class="delete-btn" @click.stop="deleteTable">
        <!-- Garbage icon -->
        <svg
          xmlns="http://www.w3.org/2000/svg"
          width="18"
          height="18"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          stroke-width="2"
          stroke-linecap="round"
          stroke-linejoin="round"
        >
          <polyline points="3 6 5 6 21 6" />
          <path d="M19 6l-1 14a2 2 0 0 1-2 2H8a2 2 0 0 1-2-2L5 6" />
          <path d="M10 11v6" />
          <path d="M14 11v6" />
          <path d="M9 6V4a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v2" />
        </svg>
      </button>

      <!-- Title -->
      <h2 class="table-title">{{ tableCard.tableName }}</h2>

      <div class="divider"></div>

      <!-- Info -->
      <div class="info">
        <div class="info-row">
          <span class="label">Primary Key:</span>
          <span class="value">{{ tableCard.primaryKey || "-" }}</span>
        </div>
      </div>

      <!-- Data button -->
      <button @click="tableDataButtonClick(tableCard.tableName)" class="data-btn">DATA →</button>
    </div>
  </div>
</template>

<script>
import Swal from "sweetalert2";

export default {
  props: {
    tableCard: {
      type: Object,
      required: true,
    },
    databaseName: {
      type: String,
      required: true,
    },
  },

  methods: {
    tableDataButtonClick(tableName) {
      this.$router.push({
        path: "/DBview/TablesView/TableData",
        query: { tableName, databaseName: this.databaseName },
      });
    },

    async deleteTable() {
      const result = await Swal.fire({
        title: "Delete Table?",
        text: `Are you sure you want to delete "${this.tableCard.tableName}"?`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6",
        confirmButtonText: "Yes, delete it",
      });

      if (!result.isConfirmed) return;
      this.$emit("deleteTable", this.tableCard.tableName);
    },
  },
};
</script>

<style scoped>
@import url("https://fonts.googleapis.com/css2?family=Nunito:wght@400;600;700&display=swap");

/* Wrapper */
.tablecard-wrapper {
  display: flex;
  justify-content: center;
  align-items: center;
  margin: 12px;
  width: 100%;
}

/* Card */
.tablecard {
  position: relative;
  width: 340px;
  height: 200px;
  background: rgba(45, 55, 90, 0.55);
  backdrop-filter: blur(10px);
  border-radius: 14px;
  padding: 20px 24px;
  border: 1px solid rgba(255, 100, 180, 0.4);
  box-shadow: 0 0 18px rgba(255, 120, 200, 0.25);
  transition: 0.25s ease;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  overflow: hidden;
}

.tablecard:hover {
  transform: translateY(-4px);
  box-shadow: 0 0 25px rgba(255, 120, 200, 0.55);
  border-color: #ff8ede;
  background: rgba(55, 65, 110, 0.65);
}

/* Delete button */
.delete-btn {
  position: absolute;
  top: 12px;
  right: 12px;
  width: 36px;
  height: 36px;
  border-radius: 10px;
  border: none;
  background: rgba(255, 110, 200, 0.15);
  color: #ff9ce6;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s ease;
}

.delete-btn:hover {
  background: rgba(255, 110, 200, 0.35);
  color: #2d1f2c;
  box-shadow: 0 0 14px rgba(255, 120, 200, 0.6);
  transform: scale(1.05);
}

/* Title */
.table-title {
  color: pink;
  font-size: 1.6em;
  font-weight: 700;
  text-align: center;
  margin: 0;
  margin-bottom: 8px;
  line-height: 1.2em;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

/* Divider */
.divider {
  height: 1px;
  width: 100%;
  background: #ff9ce6;
  opacity: 0.6;
  margin-bottom: 12px;
}

/* Info */
.info {
  display: flex;
  flex-direction: column;
}

.info-row {
  display: flex;
  justify-content: space-between;
}

.label {
  color: #ffb5eb;
  font-weight: 700;
  font-size: 1em;
}

.value {
  color: #f3f3f3;
  font-size: 1em;
}

/* Data button */
.data-btn {
  margin-top: 10px;
  width: 100%;
  padding: 10px;
  border-radius: 8px;
  border: none;
  background: linear-gradient(90deg, #ff6ec7, #ff9ce6);
  color: #2d1f2c;
  font-weight: 700;
  font-size: 1em;
  cursor: pointer;
  transition: 0.2s ease;
  box-shadow: 0 0 10px rgba(255, 110, 200, 0.4);
}

.data-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 0 18px rgba(255, 120, 200, 0.8);
}
</style>
