<template>
  <div v-if="visible" class="modal-overlay" @click.self="closeModal">
    <div class="modal-box">
      <h2 class="modal-title">Query Result</h2>
      <div class="divider"></div>

      <div class="table-wrapper">
        <table class="styled-table">
          <thead>
            <tr>
              <th v-for="(header, index) in tableHeaders" :key="index">
                {{ header }}
              </th>
            </tr>
          </thead>

          <tbody>
            <tr v-for="(row, rowIndex) in tableData" :key="rowIndex">
              <td v-for="header in tableHeaders" :key="header">
                <span v-if="editRow !== rowIndex">{{ row[header] }}</span>
                <input v-else v-model="editableRow[header]" class="edit-input" />
                <input
                  v-if="insertRow === rowIndex"
                  v-model="insertableRow[header]"
                  class="edit-input"
                />
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div class="modal-actions">
        <button type="button" class="btn btn-cancel" @click="closeModal">Close</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "TableModal",
  props: {
    data: Array,
  },
  data() {
    return {
      visible: false,
      tableData: [],
      tableHeaders: [],
    };
  },
  watch: {
    data: {
      immediate: true,
      handler(newData) {
        if (newData && newData.length > 0) {
          this.tableData = newData;
          this.tableHeaders = Object.keys(newData[0]);
        }
      },
    },
  },
  methods: {
    showModal() {
      this.visible = true;
    },
    closeModal() {
      this.visible = false;
    },
  },
};
</script>

<style scoped>
@import url("https://fonts.googleapis.com/css2?family=Nunito:wght@400;600;700&display=swap");

/* Overlay */
.modal-overlay {
  position: fixed;
  inset: 0;
  backdrop-filter: blur(6px);
  background: rgba(0, 0, 0, 0.65);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 3000;
}

/* Modal Card */
.modal-box {
  max-height: 80vh;
  overflow-y: auto;
  width: fit-content;
  background: rgba(45, 55, 90, 0.55);
  backdrop-filter: blur(12px);
  border-radius: 16px;
  padding: 28px 30px;
  border: 1px solid rgba(255, 100, 180, 0.4);
  box-shadow: 0 0 22px rgba(255, 120, 200, 0.35);
  transition: 0.25s ease;
  animation: fadeIn 0.25s ease-out;
}

.modal-box:hover {
  box-shadow: 0 0 32px rgba(255, 120, 200, 0.6);
  border-color: #ff8ede;
}

/* Title */
.modal-title {
  color: pink;
  font-size: 1.9em;
  font-weight: 700;
  text-align: center;
  text-shadow: 0 0 14px rgba(255, 150, 200, 0.6);
  margin: 0 0 14px;
}

/* Divider */
.divider {
  height: 1px;
  width: 100%;
  background: #ff9ce6;
  opacity: 0.6;
  margin-bottom: 16px;
}

/* Query Textarea */
.query-input {
  width: 100%;
  height: 220px;
  background-color: #2e3a62;
  color: #ffe6fb;
  border: 1px solid rgba(255, 100, 180, 0.45);
  border-radius: 10px;
  padding: 14px;
  font-size: 16px;
  resize: none;
  outline: none;
  font-family: "Nunito", sans-serif;
}

.query-input:focus {
  border-color: #ff8ede;
  box-shadow: 0 0 6px rgba(255, 120, 200, 0.6);
}

/* Buttons */
.modal-actions {
  margin-top: 18px;
  display: flex;
  justify-content: flex-end;
  gap: 12px;
}

/* Cancel Button (Outlined, soft glow) */
.btn-cancel {
  background: transparent;
  border: 1px solid #ff9ce6;
  color: #ffb5eb;
  padding: 10px 18px;
  border-radius: 10px;
  font-size: 1em;
  font-weight: 700;
  cursor: pointer;
  transition: all 0.2s ease;
}

.btn-cancel:hover {
  background: rgba(255, 140, 220, 0.15);
  box-shadow: 0 0 12px rgba(255, 140, 220, 0.5);
}

/* Run Button (Gradient) */
.btn-run {
  background: linear-gradient(90deg, #ff6ec7, #ff9ce6);
  border: none;
  padding: 10px 22px;
  border-radius: 10px;
  font-weight: 700;
  font-size: 1em;
  cursor: pointer;
  color: #2d1f30;
  transition: all 0.25s ease;
  box-shadow: 0 0 12px rgba(255, 120, 200, 0.45);
}

.btn-run:hover {
  transform: translateY(-2px);
  box-shadow: 0 0 24px rgba(255, 140, 220, 0.85);
}

/* Animation */
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(10px);
  }

  to {
    opacity: 1;
    transform: translateY(0);
  }
}
.styled-table th,
.styled-table td {
  padding: 10px;
  border-bottom: 1px solid #ffffff20;
}
.table-wrapper {
  max-height: 400px; /* Set a maximum height for the table container */
  overflow-y: auto; /* Add vertical scrolling if content overflows */
  overflow-x: auto; /* Add horizontal scrolling if content overflows */
  margin-bottom: 16px;
  /* Center the table */
  display: flex;
  justify-content: center; /* Center horizontally */
  align-items: center;
}
.table-wrapper::-webkit-scrollbar {
  width: 8px; /* Width of the scrollbar */
}

.table-wrapper::-webkit-scrollbar-thumb {
  background: rgba(255, 100, 180, 0.6); /* Color of the scrollbar thumb */
  border-radius: 4px; /* Rounded corners */
}

.table-wrapper::-webkit-scrollbar-thumb:hover {
  background: rgba(255, 100, 180, 0.8); /* Darker color on hover */
}

.table-wrapper::-webkit-scrollbar-track {
  background: rgba(45, 55, 90, 0.3); /* Background of the scrollbar track */
}
</style>
