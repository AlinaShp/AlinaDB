<template>
  <!-- Modal Background -->
  <div v-if="visible" class="modal-overlay" @click.self="closeModal">
    <!-- Modal Box -->
    <div class="modal-box">
      <h2 class="modal-title">Create New Database</h2>

      <div class="divider"></div>

      <!-- Database Name -->
      <label class="modal-label">Database Name</label>
      <input
        v-model="dbName"
        placeholder="MyNewDatabase"
        class="modal-input"
      />

      <!-- Collation -->
      <label class="modal-label">Collation</label>
      <select v-model="collation" class="modal-input">
        <option value="SQL_Latin1_General_CP1_CI_AS">
          SQL_Latin1_General_CP1_CI_AS (Default)
        </option>
        <option value="Latin1_General_100_CI_AS_SC">
          Latin1_General_100_CI_AS_SC
        </option>
        <option value="Hebrew_100_CI_AS">
          Hebrew_100_CI_AS
        </option>
      </select>

      <!-- Initial Size -->
      <label class="modal-label">Initial Size (MB)</label>
      <input
        type="number"
        min="10"
        v-model.number="initialSize"
        class="modal-input"
      />

      <!-- Auto Growth -->
      <div class="checkbox-row">
        <input type="checkbox" id="autogrowth" v-model="autoGrowth" />
        <label for="autogrowth">Enable Auto Growth</label>
      </div>

      <!-- Buttons -->
      <div class="modal-actions">
        <button class="btn btn-cancel" @click="closeModal">
          Cancel
        </button>
        <button class="btn btn-run" @click="createDB">
          Create
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import Swal from "sweetalert2";
import { createDatabase } from "../api/DBApi";

export default {
  name: "NewDbModal",
  props: {
    instanceName: String
  },
  data() {
    return {
      visible: false,
      dbName: "",
      collation: "SQL_Latin1_General_CP1_CI_AS",
      initialSize: 50,
      autoGrowth: true,
    };
  },
  methods: {
    showModal() {
      this.visible = true;
    },
    closeModal() {
      this.visible = false;
      this.reset();
    },
    reset() {
      this.dbName = "";
      this.collation = "SQL_Latin1_General_CP1_CI_AS";
      this.initialSize = 50;
      this.autoGrowth = true;
    },
    async createDB() {
      if (!this.dbName) {
        Swal.fire({
          icon: "warning",
          title: "Database name required",
          text: "Please enter a name for the new database.",
        });
        return;
      }

      const payload = {
        databaseName: this.dbName,
        collation: this.collation,
        initialSizeMB: this.initialSize,
        autoGrowth: this.autoGrowth,
      };

      try {
        Swal.fire({
          title: "Creating database...",
          allowOutsideClick: false,
          didOpen: () => {
            Swal.showLoading();
          },
        });

        await createDatabase(this.instanceName, payload);

        Swal.fire({
          icon: "success",
          title: "Database Created",
          text: `Database "${this.dbName}" has been created successfully.`,
        });

        this.closeModal();

      } catch (error) {
        console.error(error);
        Swal.fire({
          icon: "error",
          title: "Creation Failed",
          text: `Could not create the database.`,
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
