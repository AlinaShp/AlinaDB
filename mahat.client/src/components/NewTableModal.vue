<template>
  <div v-if="visible" class="modal-overlay" @click.self="closeModal">
    <div class="modal-box">
      <h2 class="modal-title">Create New Table</h2>
      <div class="divider"></div>

      <!-- Database -->
      <label class="modal-label">Database</label>
      <select v-model="database" class="modal-input" @change="changeDB">
        <option disabled value="">Select database...</option>
        <option v-for="database in databases" :key="database" :value="database">
          {{ database }}
        </option>
      </select>

      <!-- Table name -->
      <label class="modal-label">Table Name</label>
      <input v-model="tableName" class="modal-input" placeholder="e.g. Employees" />

      <!-- Columns -->
      <label class="modal-label">Columns</label>

      <div v-for="(col, index) in columns" :key="index" class="column-row">
        <input v-model="col.colName" class="column-input" placeholder="Column name" />

        <select v-model="col.dataType" class="column-select">
          <option>INT</option>
          <option>VARCHAR(255)</option>
          <option>NVARCHAR(255)</option>
          <option>DATE</option>
          <option>DATETIME</option>
          <option>BIT</option>
        </select>

        <label class="checkbox">
          <input type="checkbox" v-model="col.isNullable" />
          Nullable
        </label>
        <label class="checkbox">
          <input type="checkbox" v-model="col.isUnique" />
          Unique
        </label>
        <label class="checkbox" v-if="!col.isPrimaryKey">
          <input type="checkbox" v-model="col.isForeignKey" />
          Foriegn Key
        </label>
        <label class="checkbox" v-if="!col.isForeignKey">
          <input type="checkbox" v-model="col.isPrimaryKey" @change="changePrimary(col)" />
          Primary Key
        </label>
        <label class="checkbox" v-if="col.isPrimaryKey">
          <input type="checkbox" v-model="col.isIdentity" />
          Identity
        </label>

        <!-- Table -->
        <select
          v-model="col.foreignKeyTableName"
          class="modal-input-foreign"
          v-if="col.isForeignKey && database"
          @change="changeTable(col)"
        >
          <option disabled value="">Tables</option>
          <option v-for="foriegnTable in foriegnTables" :key="foriegnTable" :value="foriegnTable">
            {{ foriegnTable }}
          </option>
        </select>

        <!-- Column -->
        <select
          v-model="col.foreignKeyColName"
          class="modal-input-foreign"
          v-if="col.foreignKeyTableName && col.isForeignKey && database"
        >
          <option disabled value="">Columns</option>
          <option
            v-for="foriegnColumn in foriegnColumns"
            :key="foriegnColumn"
            :value="foriegnColumn"
          >
            {{ foriegnColumn }}
          </option>
        </select>

        <button class="remove-btn" @click="removeColumn(index)">✕</button>
      </div>

      <button class="add-column-btn" @click="addColumn">+ Add Column</button>

      <!-- Actions -->
      <div class="modal-actions">
        <button class="btn btn-cancel" @click="closeModal">Cancel</button>
        <button class="btn btn-run" @click="createTable">Create Table</button>
      </div>
    </div>
  </div>
</template>

<script>
import Swal from "sweetalert2";
import { addTable, tablesInfo, getTableColumns } from "../api/TableApi";
import { getDBinfo } from "../api/DBApi";

export default {
  name: "NewTableModal",

  props: {
    instanceName: String,
  },

  data() {
    return {
      visible: false,
      databases: [],
      foriegnTables: [],
      foriegnColumns: [],
      columns: [
        {
          colName: "Id",
          dataType: "INT",
          isNullable: false,
          isForeignKey: false,
          isUnique: false,
          isPrimaryKey: false,
          isIdentity: false,
          foreignKeyTableName: "",
          foreignKeyColName: "",
        },
      ],
    };
  },

  methods: {
    async showModal() {
      this.visible = true;
      await this.loadDatabases();
    },

    closeModal() {
      this.visible = false;
      this.database = "";
      this.tableName = "";
      this.columns = [
        {
          colName: "Id",
          dataType: "INT",
          isNullable: false,
          isForeignKey: false,
          isUnique: false,
          isPrimaryKey: false,
          isIdentity: false,
          foreignKeyTableName: "",
          foreignKeyColName: "",
        },
      ];
      this.databases = [];
      this.foriegnTables = [];
      this.foriegnColumns = [];
    },

    async loadDatabases() {
      // TEST

      // AXIOS
      try {
        const response = await getDBinfo(this.instanceName);
        this.databases = response.data.map((db) => db.databaseName);
      } catch (error) {
        Swal.fire({
          icon: "error",
          title: "Failed to load databases",
          text: "Could not retrieve database list.",
        });

        console.error(error);
      }
    },

    addColumn() {
      this.columns.push({
        colName: "",
        dataType: "VARCHAR(255)",
        isNullable: false,
        isForeignKey: false,
        isUnique: false,
        isPrimaryKey: false,
        isIdentity: false,
        foreignKeyTableName: "",
        foreignKeyColName: "",
      });
    },

    removeColumn(index) {
      this.columns.splice(index, 1);
    },

    changePrimary(col) {
      this.columns.forEach((column) => {
        column.isPrimaryKey = false;
      });
      col.isPrimaryKey = true;
    },

    async loadTabales() {
      try {
        const response = await tablesInfo(this.database, this.instanceName);
        this.foriegnTables = response.data.map((db) => db.tableName);
      } catch (error) {
        Swal.fire({
          icon: "error",
          title: "Failed to load table",
          text: "Could not retrieve table list.",
        });

        console.error(error);
      }
    },

    async loadColumns(col) {
      try {
        debugger;
        const response = await getTableColumns(
          this.database,
          col.foreignKeyTableName,
          this.instanceName,
        );
        this.foriegnColumns = response.data.map((column) => column.colName);
      } catch (error) {
        Swal.fire({
          icon: "error",
          title: "Failed to load column",
          text: "Could not retrieve column list.",
        });

        console.error(error);
      }
    },

    async changeDB() {
      this.columns.forEach((col) => {
        col.foreignKeyColName = "";
        col.foreignKeyTableName = "";
      });
      this.foriegnColumns = [];
      this.foriegnTables = [];

      if (this.database) {
        await this.loadTabales();
      }
    },

    async changeTable(col) {
      col.foreignKeyColName = "";
      this.foriegnColumns = [];

      if (col.foreignKeyTableName) {
        await this.loadColumns(col);
      }
    },

    async createTable() {
      debugger;
      if (!this.database || !this.tableName) {
        Swal.fire({
          icon: "warning",
          title: "Missing information",
          text: "Database and table name are required.",
        });
        return;
      }

      if (this.columns.some((c) => !c.colName)) {
        Swal.fire({
          icon: "warning",
          title: "Invalid columns",
          text: "All columns must have names.",
        });
        return;
      }

      const payload = {
        tableName: this.tableName,
        cols: this.columns,
      };

      try {
        Swal.fire({
          title: "Creating table...",
          allowOutsideClick: false,
          didOpen: () => {
            Swal.showLoading();
          },
        });

        await addTable(this.database, this.instanceName, payload);

        Swal.fire({
          icon: "success",
          title: "Table Created",
          text: `Table "${this.tableName}" created successfully.`,
        });

        this.closeModal();
      } catch (error) {
        const msg = error.response?.data?.message || error.message;

        Swal.fire({
          icon: "error",
          title: "Table creation failed",
          text: msg,
        });

        console.error(error);
      }
    },
  },
};
</script>

<style scoped>
/* Overlay */
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(10, 10, 30, 0.75);
  backdrop-filter: blur(6px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 3000;
}

/* Modal box (SCROLL FIX HERE) */
.modal-box {
  width: 1100px;
  max-width: 95%;
  max-height: 85vh; /* LIMIT HEIGHT */
  overflow-y: auto; /* ENABLE SCROLL */
  background: rgba(45, 55, 90, 0.65);
  backdrop-filter: blur(12px);
  border-radius: 14px;
  padding: 30px;
  border: 1px solid rgba(255, 140, 220, 0.45);
  box-shadow: 0 0 30px rgba(255, 120, 200, 0.35);
}

/* Nice scrollbar (optional but recommended) */
.modal-box::-webkit-scrollbar {
  width: 8px;
}

.modal-box::-webkit-scrollbar-track {
  background: transparent;
}

.modal-box::-webkit-scrollbar-thumb {
  background: linear-gradient(#ff6ec7, #ff9ce6);
  border-radius: 10px;
}

/* Title */
.modal-title {
  color: #ff9ce6;
  font-size: 30px;
  font-weight: 800;
  text-align: center;
  margin-bottom: 10px;
  text-shadow: 0 0 14px rgba(255, 150, 220, 0.7);
}

/* Divider */
.divider {
  height: 1px;
  background: linear-gradient(90deg, transparent, #ff9ce6, transparent);
  margin-bottom: 20px;
}

/* Labels */
.modal-label {
  color: #ffd3f2;
  font-size: 16px;
  font-weight: 600;
  margin-bottom: 6px;
  display: block;
}

/* Inputs */
.modal-input {
  width: 100%;
  background: rgba(25, 35, 65, 0.9);
  color: white;
  border: 1px solid rgba(255, 140, 220, 0.45);
  border-radius: 8px;
  padding: 12px;
  font-size: 16px;
  margin-bottom: 14px;
  outline: none;
}

.modal-input:focus {
  border-color: #ff9ce6;
  box-shadow: 0 0 8px rgba(255, 140, 220, 0.7);
}
/* Inputs */
.modal-input-foreign {
  width: fit-content;
  background: rgba(25, 35, 65, 0.9);
  color: white;
  border: 1px solid rgba(255, 140, 220, 0.45);
  border-radius: 8px;
  padding: 12px;
  font-size: 16px;
  margin-bottom: 14px;
  outline: none;
}

.modal-input-foreign:focus {
  border-color: #ff9ce6;
  box-shadow: 0 0 8px rgba(255, 140, 220, 0.7);
}

/* Column rows */
.column-row {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr auto;
  gap: 10px;
  margin-bottom: 10px;
}

.column-input,
.column-select {
  background: rgba(25, 35, 65, 0.9);
  border: 1px solid rgba(255, 140, 220, 0.45);
  border-radius: 8px;
  padding: 10px;
  color: white;
  width: fit-content;
}

/* Nullable checkbox */
.checkbox {
  display: flex;
  align-items: center;
  gap: 6px;
  color: #ffd3f2;
  font-size: 14px;
}

/* Remove column */
.remove-btn {
  background: transparent;
  border: none;
  color: #ff9ce6;
  font-size: 18px;
  cursor: pointer;
}

.remove-btn:hover {
  color: #ff6ec7;
}

/* Add column */
.add-column-btn {
  margin-top: 6px;
  background: transparent;
  border: 1px dashed #ff9ce6;
  color: #ff9ce6;
  padding: 8px;
  width: 100%;
  border-radius: 8px;
  cursor: pointer;
}

.add-column-btn:hover {
  background: rgba(255, 140, 220, 0.12);
}

/* Actions */
.modal-actions {
  margin-top: 24px;
  display: flex;
  justify-content: flex-end;
  gap: 14px;
}

/* Buttons */
.btn {
  padding: 10px 22px;
  font-size: 16px;
  font-weight: 700;
  border-radius: 10px;
  cursor: pointer;
  transition: all 0.25s ease;
}

/* Cancel */
.btn-cancel {
  background: transparent;
  border: 1px solid #ff9ce6;
  color: #ff9ce6;
}

.btn-cancel:hover {
  background: rgba(255, 140, 220, 0.15);
  box-shadow: 0 0 12px rgba(255, 140, 220, 0.5);
}

/* Run */
.btn-run {
  background: linear-gradient(90deg, #ff6ec7, #ff9ce6);
  border: none;
  color: #2b1f30;
  box-shadow: 0 0 16px rgba(255, 140, 220, 0.6);
}

.btn-run:hover {
  transform: translateY(-2px);
  box-shadow: 0 0 24px rgba(255, 160, 230, 0.9);
}
</style>
