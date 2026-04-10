<template>
  <div class="dbcard-wrapper">
    <div class="dbcard">
      <!-- Delete DB Button -->
      <button class="delete-btn" title="Delete database" @click="deleteDb">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          stroke-width="1.8"
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
      <h2 class="db-title">{{ card.databaseName }}</h2>

      <div class="divider"></div>

      <div class="info">
        <div class="info-row">
          <span class="label">ID:</span>
          <span class="value">{{ card.databaseId }}</span>
        </div>
        <div class="info-row">
          <span class="label">Recovery Model:</span>
          <span class="value">{{ card.recoveryModel }}</span>
        </div>
        <div class="info-row">
          <span class="label">State:</span>
          <span class="value">{{ card.state }}</span>
        </div>
        <div class="info-row">
          <span class="label">Collation:</span>
          <span class="value">{{ card.collation }}</span>
        </div>
      </div>

      <!-- Tables Button -->
      <button @click="tableButtonClick(card.databaseName)" class="tables-btn">TABLES →</button>
    </div>
  </div>
</template>

<script>
import Swal from "sweetalert2";

export default {
  props: {
    card: {
      type: Object,
      required: true,
    },
  },

  methods: {
    tableButtonClick(dbName) {
      this.$emit("buttonClick", this.card.databaseName);
    },

    async deleteDb() {
      const result = await Swal.fire({
        title: "Delete Database?",
        text: `Are you sure you want to delete "${this.card.DatabaseName}"?`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6",
        confirmButtonText: "Yes, delete it",
      });

      if (!result.isConfirmed) return;
      this.$emit("delete", this.card);
    },
  },
};
</script>

<style scoped>
@import url("https://fonts.googleapis.com/css2?family=Nunito:wght@400;600;700&display=swap");

/* Wrapper */
.dbcard-wrapper {
  display: flex;
  justify-content: center;
  align-items: center;
}

/* Card */
.dbcard {
  position: relative;
  width: 340px;
  height: 360px;
  background: rgba(45, 55, 90, 0.55);
  backdrop-filter: blur(10px);
  border-radius: 14px;
  padding: 26px 28px;
  border: 1px solid rgba(255, 100, 180, 0.4);
  box-shadow: 0 0 18px rgba(255, 120, 200, 0.25);
  transition: 0.25s ease;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  font-family: "Nunito", sans-serif;
}

.dbcard:hover {
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
.db-title {
  color: pink;
  font-size: 1.9em;
  font-weight: 700;
  text-shadow: 0 0 12px rgba(255, 150, 200, 0.6);
  text-align: center;
  margin: 0 0 12px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
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
  flex-grow: 1;
}

.info-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 6px;
}

.label {
  color: #ffb5eb;
  font-weight: 700;
}

.value {
  color: #f3f3f3;
  font-weight: 400;
  text-align: right;
}

/* Tables button */
.tables-btn {
  margin-top: 12px;
  width: 100%;
  padding: 12px;
  border-radius: 8px;
  border: none;
  background: linear-gradient(90deg, #ff6ec7, #ff9ce6);
  color: #2d1f2c;
  font-weight: 700;
  font-size: 1.15em;
  cursor: pointer;
  transition: 0.2s ease;
  box-shadow: 0 0 10px rgba(255, 110, 200, 0.4);
}

.tables-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 0 18px rgba(255, 120, 200, 0.8);
}
</style>
