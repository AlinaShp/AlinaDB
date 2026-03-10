<template>
  <!-- Modal Background -->
  <div v-if="visible" class="modal-overlay" @click.self="closeModal">
    <!-- Modal Box -->
    <div class="modal-box">
      <h2 class="modal-title">Restore Database</h2>

      <div class="divider"></div>

      <!-- Database Selector -->
      <label class="modal-label">Database</label>
      <select v-model="selectedDatabase" class="modal-input">
        <option disabled value="">Select database...</option>
        <option v-for="db in databases" :key="db" :value="db">
          {{ db }}
        </option>
      </select>

      <!-- Backup Path -->
      <label class="modal-label">Backup File Path</label>
      <input v-model="backupPath" placeholder="C:\\Backups\\MyDatabase.bak" class="modal-input" />

      <!-- Overwrite -->
      <div class="checkbox-row">
        <input type="checkbox" id="overwrite" v-model="overwrite" />
        <label for="overwrite">Overwrite existing database</label>
      </div>

      <!-- Buttons -->
      <div class="modal-actions">
        <button class="btn btn-cancel" @click="closeModal">Cancel</button>
        <button class="btn btn-run" @click="runRestore">Restore</button>
      </div>
    </div>
  </div>
</template>

<script>
import Swal from "sweetalert2";
import { restoreDatabase, getDBinfo } from "../api/DBApi";
export default {
  name: "RestoreModal",
  props: {
    instanceName: String,
  },
  data() {
    return {
      visible: false,
      selectedDatabase: "",
      backupPath: "",
      overwrite: false,
      databases: [],
    };
  },
  methods: {
    showModal() {
      this.visible = true;
      this.loadDatabases();
    },
    closeModal() {
      this.visible = false;
      this.selectedDatabase = "";
      this.backupPath = "";
      this.overwrite = false;
    },
    async loadDatabases() {
      // TEST
      this.databases = ["AdventureWorks2022", "master", "ReportDB"];
      // AXIOS
      try {
        const response = await getDBinfo(this.instanceName);

        this.databases = response.data.map((db) => db.databaseName);
        console.error(error);

        const msg =
          error.response?.data?.message ||
          error.message;

      } catch (error) {
        Swal.fire({
          icon: "error",
          title: "Failed to load databases",
          text: msg,
        });

        console.error(error);
      }
    },

    async runRestore() {
      if (!this.selectedDatabase || !this.backupPath) {
        Swal.fire({
          icon: "warning",
          title: "Missing fields",
          text: "Database and backup path are required.",
        });

        return;
      }

      try {
        Swal.fire({
          title: "Restoring database...",
          text: "Please wait",
          allowOutsideClick: false,
          didOpen: () => {
            Swal.showLoading();
          },
        });

        await restoreDatabase(this.selectedDatabase, this.instanceName);

        Swal.fire({
          icon: "success",
          title: "Restore completed",
          text: `${this.selectedDatabase} restored successfully`,
        });

        this.closeModal();
      } catch (error) {

        console.error(error);

        const msg =
          error.response?.data?.message ||
          error.message;

        Swal.fire({
          icon: "error",
          title: "Restore failed",
          text: msg,
        });

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

/* Modal */
.modal-box {
  width: 720px;
  max-width: 92%;
  background: rgba(45, 55, 90, 0.65);
  backdrop-filter: blur(12px);
  border-radius: 14px;
  padding: 30px;
  border: 1px solid rgba(255, 140, 220, 0.45);
  box-shadow: 0 0 30px rgba(255, 120, 200, 0.35);
  transition: all 0.25s ease;
}

.modal-box:hover {
  box-shadow: 0 0 45px rgba(255, 140, 220, 0.65);
  border-color: #ff9ce6;
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
  color: #ffffff;
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

/* Checkbox */
.checkbox-row {
  display: flex;
  align-items: center;
  gap: 10px;
  color: #ffd3f2;
  margin-top: 6px;
  font-size: 15px;
}

/* Buttons */
.modal-actions {
  margin-top: 26px;
  display: flex;
  justify-content: flex-end;
  gap: 14px;
}

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
