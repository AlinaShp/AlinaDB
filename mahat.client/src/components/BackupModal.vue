<template>
  <!-- Modal Background -->
  <div v-if="visible" class="modal-overlay" @click.self="closeModal">
    <!-- Modal Box -->
    <div class="modal-box">
      <h2 class="modal-title">Backup Database</h2>
      <div class="divider"></div>

      <!-- Database Selector -->
      <label class="modal-label">Database</label>
      <select v-model="selectedDatabase" class="modal-input">
        <option disabled value="">Select a database...</option>
        <option v-for="db in databases" :key="db">{{ db }}</option>
      </select>

      <!-- Backup Type Selector -->
      <label class="modal-label">Backup Type</label>
      <select v-model="backupType" class="modal-input">
        <option value="FULL">Full</option>
        <option value="DIFFERENTIAL">Differential</option>
        <option value="LOG">Transaction Log</option>
      </select>

      <!-- Backup Path -->
      <label class="modal-label">Backup File Path</label>
      <input type="text" v-model="backupPath" placeholder="C:\Backups\" class="modal-input" />

      <!-- File Name -->
      <label class="modal-label">Backup File Name</label>
      <input type="text" v-model="backupFile" placeholder="MyDatabaseBackup" class="modal-input" />

      <!-- Actions -->
      <div class="modal-actions">
        <button class="btn btn-cancel" @click="closeModal">Cancel</button>
        <button class="btn btn-run" @click="runBackup">Backup</button>
      </div>
    </div>
  </div>
</template>

<script>
import Swal from "sweetalert2";
import { backupDatabase, getDBinfo } from "../api/DBApi";

export default {
  name: "BackupModal",
  props: {
    instanceName: String,
  },
  data() {
    return {
      visible: false,
      databases: [],
      selectedDatabase: "",
      backupType: "FULL",
      backupPath: "C:\\Backups\\",
      backupFile: "",
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
      this.backupType = "FULL";
      this.backupPath = "";
      this.backupFile = "";
    },
    async loadDatabases() {
      //TEST
      this.databases = ["AdventureWorks2022", "master", "ReportDB"];
      //AXIOS
      try {
        const response = await getDBinfo(this.instanceName);

        // assuming API returns list of DB objects
        this.databases = response.data.map((db) => db.databaseName);
      } catch (error) {
        console.error(error);

        Swal.fire({
          icon: "error",
          title: "Failed to load databases",
          text: "Could not retrieve database list.",
        });
      }
    },

    async runBackup() {
      if (!this.selectedDatabase || !this.backupPath || !this.backupFile) {
        Swal.fire({
          icon: "warning",
          title: "Missing fields",
          text: "All fields are required.",
        });

        return;
      }

      console.log("Simulated Backup:", {
        database: this.selectedDatabase,
        type: this.backupType,
        path: this.backupPath,
        fileName: this.backupFile,
      });

      try {
        Swal.fire({
          title: "Running backup...",
          text: "Please wait",
          allowOutsideClick: false,
          didOpen: () => {
            Swal.showLoading();
          },
        });
        debugger;
        const payload = {
          backupType: this.backupType,
          filePath: this.backupPath,
          fileName: this.backupFile,
        };

        await backupDatabase(this.selectedDatabase, this.instanceName, payload);

        Swal.fire({
          icon: "success",
          title: "Backup completed",
          text: `${this.selectedDatabase} backed up successfully`,
        });

        this.closeModal();
      } catch (error) {
        Swal.fire({
          icon: "error",
          title: "Backup failed",
          text: "The database backup could not be completed.",
        });

        console.error(error);
      }
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
  background: rgba(10, 15, 40, 0.65);
  backdrop-filter: blur(6px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 3000;
}

/* Modal Box */
.modal-box {
  width: 640px;
  background: rgba(45, 55, 90, 0.6);
  backdrop-filter: blur(12px);
  border-radius: 14px;
  padding: 28px 30px;
  border: 1px solid rgba(255, 120, 200, 0.35);
  box-shadow: 0 0 30px rgba(255, 110, 200, 0.35);
  animation: fadeIn 0.25s ease-out;
}

/* Title */
.modal-title {
  text-align: center;
  font-size: 28px;
  font-weight: 700;
  color: #ff9ce6;
  text-shadow: 0 0 14px rgba(255, 150, 200, 0.6);
  margin-bottom: 10px;
}

/* Divider */
.divider {
  height: 1px;
  width: 100%;
  background: linear-gradient(90deg, transparent, #ff9ce6, transparent);
  margin-bottom: 18px;
}

/* Labels */
.modal-label {
  display: block;
  margin-bottom: 6px;
  font-weight: 600;
  color: #ffb5eb;
}

/* Inputs */
.modal-input {
  width: 100%;
  padding: 12px;
  border-radius: 8px;
  background: rgba(30, 40, 75, 0.9);
  border: 1px solid rgba(255, 120, 200, 0.4);
  color: #f4f4f4;
  font-size: 15px;
  margin-bottom: 14px;
  outline: none;
  transition: 0.2s ease;
}

.modal-input:focus {
  border-color: #ff9ce6;
  box-shadow: 0 0 8px rgba(255, 150, 200, 0.6);
}

/* Actions */
.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
  margin-top: 22px;
}

/* Buttons (UX/UI) */
.btn {
  padding: 10px 22px;
  border-radius: 10px;
  font-size: 15px;
  font-weight: 700;
  cursor: pointer;
  border: none;
  transition: 0.2s ease;
}

/* Cancel Button */
.btn-cancel {
  background: transparent;
  border: 1px solid #ff9ce6;
  color: #ffb5eb;
}

.btn-cancel:hover {
  background: rgba(255, 140, 220, 0.15);
  box-shadow: 0 0 12px rgba(255, 140, 220, 0.5);
}

/* Backup/Run Button */
.btn-run {
  background: linear-gradient(90deg, #ff6ec7, #ff9ce6);
  color: #2d1f30;
  box-shadow: 0 0 12px rgba(255, 110, 200, 0.6);
}

.btn-run:hover {
  transform: translateY(-2px);
  box-shadow: 0 0 18px rgba(255, 120, 200, 0.85);
}

.swal-top {
  z-index: 99999 !important;
}

/* Animation */
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: scale(0.95);
  }
  to {
    opacity: 1;
    transform: scale(1);
  }
}
</style>
