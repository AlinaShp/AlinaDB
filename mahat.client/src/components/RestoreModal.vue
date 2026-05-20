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
        <div v-for="(backup, index) in backups" :key="backup.fileName">
          <p>Backup number {{ index + 1 }} in sequence:</p>

          <!-- Backup Type Selector -->
          <label class="modal-label">Backup Type</label>
          <select v-model="backup.backupType"
                  class="modal-input"
                  @change="loadBackups(backup.backupType, backup)">
            <option value="FULL" v-if="index === 0">Full</option>
            <option value="DIFFERENTIAL" v-else-if="index > 0">Differential</option>
            <option value="LOG" v-if="index > 0">Transaction Log</option>
          </select>

          <!-- Backup Path -->
          <select v-model="backup.backupPath" class="modal-input" v-if="backup.backupType">
            <option disabled value="">Backup Paths</option>
            <option v-for="backupPath in backup.allBackups" :key="backupPath" :value="backupPath">
              {{ backupPath }}
            </option>
          </select>

          <button class="remove-btn" @click="removeBackup(index)">✕</button>
        </div>

        <button class="add-backup-btn" @click="addBackup">+ Add Backup</button>

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
  import { restoreDatabase, getDBinfo, exsitingBackups } from "../api/DBApi";
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
      backups: [],
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
      this.overwrite = false
      this.backups= []  ;
    },
    async loadDatabases() {
      try {
        const response = await getDBinfo(this.instanceName);

        this.databases = response.data.map((db) => db.databaseName);
        console.error(error);

        const msg = error.response?.data?.message || error.message;
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
      debugger
      if (!this.selectedDatabase || this.backups.some((backup) => backup.backupPath === "")) {
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

        const payload = this.backups.map((backup) => ({
          backupType: backup.backupType,
          filePath: backup.backupPath.split("\\").slice(0, -1).join("\\"),
          fileName: backup.backupPath.split("\\").pop().split(".")[0],
        }));

        await restoreDatabase(this.selectedDatabase, this.instanceName, payload);

        Swal.fire({
          icon: "success",
          title: "Restore completed",
          text: `${this.selectedDatabase} restored successfully`,
        });

        this.closeModal();
      } catch (error) {
        console.error(error);

        const msg = error.response?.data?.message || error.message;

        Swal.fire({
          icon: "error",
          title: "Restore failed",
          text: msg,
        });
      }
    },

    async loadBackups(backupType, backup) {
      debugger
      try {
        const response = await exsitingBackups(
          this.selectedDatabase,
          this.instanceName,
          backupType,
        );
        backup.allBackups = response.data;
      } catch (error) {
        console.error(error);

        const msg = error.response?.data?.message || error.message;

        Swal.fire({
          icon: "error",
          title: `Failed to load backups on database ${this.selectedDatabase}`,
          text: msg,
        });
      }
    },

    addBackup() {
      this.backups.push({
        backupType: "",
        backupPath: "",
        allBackups: [],
      });
    },
    removeBackup(index) {
      this.backups.splice(index, 1)
    }
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
    max-height: 900px; /* Set a maximum height for the table container */
    overflow-y: auto; /* Add vertical scrolling if content overflows */
    overflow-x: auto; /* Add horizontal scrolling if content overflows */
    margin-bottom: 16px;
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

.add-backup-btn {
    margin-top: 6px;
    background: transparent;
    border: 1px dashed #ff9ce6;
    color: #ff9ce6;
    padding: 8px;
    width: 100%;
    border-radius: 8px;
    cursor: pointer;
  }

    .add-backup-btn:hover {
      background: rgba(255, 140, 220, 0.12);
    }
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
  .modal-box::-webkit-scrollbar {
    width: 8px; /* Width of the scrollbar */
  }

  .modal-box::-webkit-scrollbar-thumb {
    background: rgba(255, 100, 180, 0.6); /* Color of the scrollbar thumb */
    border-radius: 4px; /* Rounded corners */
  }

    .modal-box::-webkit-scrollbar-thumb:hover {
      background: rgba(255, 100, 180, 0.8); /* Darker color on hover */
    }

  .modal-box::-webkit-scrollbar-track {
    background: rgba(45, 55, 90, 0.3); /* Background of the scrollbar track */
  }
</style>
