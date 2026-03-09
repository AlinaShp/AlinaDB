<template>
  <Navbar />

  <div class="table-card">
    <h2 class="table-title">{{ tableName }}</h2>
    <div class="divider"></div>

    <!-- Toolbar -->
    <div class="table-toolbar">
      <button class="btn-add-row" @click="addRow" title="Add Row">
        <!-- Plus icon -->
        <svg
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          stroke-width="2.5"
          stroke-linecap="round"
          stroke-linejoin="round"
          class="plus-icon"
        >
          <line x1="12" y1="5" x2="12" y2="19" />
          <line x1="5" y1="12" x2="19" y2="12" />
        </svg>
        Add Row
      </button>
    </div>

    <!-- Table -->
    <div class="table-wrapper">
      <table class="styled-table">
        <thead>
          <tr>
            <th v-for="(header, index) in tableHeaders" :key="index">
              {{ header }}
            </th>
            <th class="actions-header">Actions</th>
          </tr>
        </thead>

        <tbody>
          <tr v-for="(row, rowIndex) in tableData" :key="rowIndex">
            <td v-for="header in tableHeaders" :key="header">
              <span v-if="editRow !== rowIndex">{{ row[header] }}</span>
              <input v-else v-model="editableRow[header]" class="edit-input" />
            </td>

            <!-- Actions -->
            <td class="actions-col">
              <!-- EDIT -->
              <button
                v-if="editRow !== rowIndex"
                class="action-btn edit"
                title="Edit"
                @click="toggleEdit(rowIndex)"
              >
                <!-- Pencil -->
                <svg viewBox="0 0 24 24">
                  <path d="M12 20h9" />
                  <path d="M16.5 3.5a2.1 2.1 0 0 1 3 3L7 19l-4 1 1-4Z" />
                </svg>
              </button>

              <!-- SAVE -->
              <button
                v-if="editRow === rowIndex"
                class="action-btn save"
                title="Save"
                @click="saveRow(rowIndex)"
              >
                <!-- Save -->
                <svg viewBox="0 0 24 24">
                  <path d="M19 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h11l5 5v11a2 2 0 0 1-2 2Z" />
                  <polyline points="17 21 17 13 7 13 7 21" />
                  <polyline points="7 3 7 8 15 8" />
                </svg>
              </button>

              <!-- CANCEL -->
              <button
                v-if="editRow === rowIndex"
                class="action-btn cancel"
                title="Cancel"
                @click="cancelEdit"
              >
                <!-- X -->
                <svg viewBox="0 0 24 24">
                  <line x1="18" y1="6" x2="6" y2="18" />
                  <line x1="6" y1="6" x2="18" y2="18" />
                </svg>
              </button>

              <!-- DELETE -->
              <button class="action-btn delete" title="Delete" @click="deleteRow(rowIndex)">
                <!-- Garbage -->
                <svg viewBox="0 0 24 24">
                  <polyline points="3 6 5 6 21 6" />
                  <path d="M19 6l-1 14a2 2 0 0 1-2 2H8a2 2 0 0 1-2-2L5 6" />
                  <path d="M10 11v6" />
                  <path d="M14 11v6" />
                  <path d="M9 6V4a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v2" />
                </svg>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import Navbar from "../components/Navbar.vue";
import Swal from "sweetalert2";
import { insertRow, updateRow, deleteRow, getTableData } from "@/api/TableApi";

export default {
  components: { Navbar },
  props: {
    databaseName: { type: String, required: true },
    tableName: { type: String, required: true },
  },
  data() {
    return {
      tableData: [],
      tableHeaders: [],
      editRow: null,
      editableRow: {},
      loading: true,
    };
  },
  mounted() {
    //TEST
    this.tableData = [
      { Id: 1, Name: "Product A", Price: 29.99, InStock: true },
      { Id: 2, Name: "Product B", Price: 14.5, InStock: false },
    ];
    this.tableHeaders = Object.keys(this.tableData[0]);

    //AXIOS
    this.fetchTableData();
  },
  methods: {
    async fetchTableData() {
      try {
        this.loading = true;
        const instanceName = this.$cookies.get("selectedInstance");

        // Fetch table data from API
        const response = await getTableData(this.databaseName, this.tableName, instanceName);

        this.tableData = response.data || [];

        // Extract headers dynamically from first row
        if (this.tableData.length > 0) {
          this.tableHeaders = Object.keys(this.tableData[0]);
        } else {
          this.tableHeaders = [];
        }

        this.loading = false;
      } catch (error) {
        this.loading = false;
        Swal.fire({
          icon: "error",
          title: "Failed to fetch table data",
          text: error.response?.data || error.message,
        });
      }
    },

    async addRow() {
      const row = {};
      this.tableHeaders.forEach((h) => (row[h] = ""));
      try {
        const instanceName = this.$cookies.get("selectedInstance");

        await insertRow(this.databaseName, this.tableName, instanceName, row);

        Swal.fire({
          icon: "success",
          title: "Row added",
          showConfirmButton: false,
          timer: 1200,
        });

        this.tableData.push(row);
        this.toggleEdit(this.tableData.length - 1);
      } catch (error) {
        Swal.fire({
          icon: "error",
          title: "Failed to add row",
          text: error.response?.data || error.message,
        });
      }
    },

    toggleEdit(i) {
      this.editRow = i;
      this.editableRow = { ...this.tableData[i] };
    },

    cancelEdit() {
      this.editRow = null;
      this.editableRow = {};
    },

    async saveRow(i) {
      try {
        const instanceName = this.$cookies.get("selectedInstance");
        const row = this.editableRow;
        const primaryKeyName = this.tableHeaders[0];
        const primaryKeyValue = row[primaryKeyName];

        await updateRow(
          this.databaseName,
          this.tableName,
          instanceName,
          primaryKeyName,
          primaryKeyValue,
          row,
        );

        Swal.fire({
          icon: "success",
          title: "Row saved",
          showConfirmButton: false,
          timer: 1200,
        });

        this.tableData[i] = { ...row };
        this.editRow = null;
      } catch (error) {
        Swal.fire({
          icon: "error",
          title: "Failed to save row",
          text: error.response?.data || error.message,
        });
      }
    },

    async deleteRow(i) {
      try {
        const row = this.tableData[i];
        const instanceName = this.$cookies.get("selectedInstance");
        const primaryKeyName = this.tableHeaders[0]; // first column is PK
        const primaryKeyValue = row[primaryKeyName];

        const result = await Swal.fire({
          title: "Delete row?",
          text: `Are you sure you want to delete this row?`,
          icon: "warning",
          showCancelButton: true,
          confirmButtonColor: "#d33",
          cancelButtonColor: "#3085d6",
          confirmButtonText: "Yes, delete it",
        });

        if (!result.isConfirmed) return;

        await deleteRow(
          this.databaseName,
          this.tableName,
          instanceName,
          primaryKeyName,
          primaryKeyValue,
        );

        Swal.fire({
          icon: "success",
          title: "Row deleted",
          showConfirmButton: false,
          timer: 1200,
        });

        this.tableData.splice(i, 1);
        this.editRow = null;
      } catch (error) {
        Swal.fire({
          icon: "error",
          title: "Failed to delete row",
          text: error.response?.data || error.message,
        });
      }
    },
  },
};
</script>

<style scoped>
@import url("https://fonts.googleapis.com/css2?family=Nunito:wght@400;600;700&display=swap");

.table-card {
  background: rgba(45, 55, 90, 0.55);
  backdrop-filter: blur(10px);
  border-radius: 14px;
  padding: 28px;
  margin: 30px auto;
  width: 70%;
  box-shadow: 0 0 20px rgba(255, 120, 200, 0.25);
  border: 1px solid rgba(255, 100, 180, 0.4);
  font-family: "Nunito", sans-serif;
}

.table-title {
  color: #ffc3eb;
  text-align: center;
  font-size: 2.2em;
}

.divider {
  height: 1px;
  background: #ff9ce6;
  margin-bottom: 20px;
}

.table-toolbar {
  margin-bottom: 20px;
}

.table-toolbar {
  margin-bottom: 20px;
}

.btn-add-row {
  display: flex;
  align-items: center;
  gap: 10px; /* space between icon and text */
  padding: 10px 22px;
  background: linear-gradient(90deg, #ff6ec7, #ff9ce6);
  border: none;
  border-radius: 10px;
  font-weight: 700;
  font-size: 1em;
  color: #2d1f30; /* text color */
  cursor: pointer;
  box-shadow: 0 0 12px rgba(255, 120, 200, 0.45);
  transition: all 0.25s ease;
}

.btn-add-row:hover {
  transform: translateY(-2px);
  box-shadow: 0 0 24px rgba(255, 140, 220, 0.85);
}

.plus-icon {
  width: 24px;
  height: 24px;
  stroke: #2d1f30; /* matches text color, visible on gradient */
}

.add-text {
  font-weight: bold;
}

.table-wrapper {
  overflow-x: auto;
}

.styled-table {
  width: 100%;
  border-collapse: collapse;
  color: #f3f3f3;
}

.styled-table th,
.styled-table td {
  padding: 10px;
  border-bottom: 1px solid #ffffff20;
}

.actions-col {
  text-align: center;
  width: 180px;
}

.edit-input {
  width: 100%;
  background: #2e3a62;
  color: #ffe6fb;
  border: 1px solid #fa9dea;
  border-radius: 6px;
  padding: 6px;
}

/* === ACTION BUTTONS (MATCH DELETE UX/UI) === */

.action-btn {
  width: 36px;
  height: 36px;
  border-radius: 10px;
  border: none;
  background: rgba(255, 110, 200, 0.15);
  color: #ff9ce6;
  cursor: pointer;
  margin: 0 4px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s ease;
}

.action-btn svg {
  width: 18px;
  height: 18px;
  fill: none;
  stroke: currentColor;
  stroke-width: 2;
  stroke-linecap: round;
  stroke-linejoin: round;
}

.action-btn:hover {
  background: rgba(255, 110, 200, 0.35);
  color: #2d1f2c;
  box-shadow: 0 0 14px rgba(255, 120, 200, 0.6);
  transform: scale(1.08);
}

/* Color accents */
.action-btn.edit {
  color: #9dd0ff;
}
.action-btn.save {
  color: #9dffd6;
}
.action-btn.cancel {
  color: #ffd39d;
}
.action-btn.delete {
  color: #ff9d9d;
}
</style>
