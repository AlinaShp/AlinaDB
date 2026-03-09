<template>
  <!-- Modal Background -->
  <div v-if="visible" class="modal-overlay" @click.self="closeModal">
    <!-- Modal Box -->
    <div class="modal-box">
      <h2 class="modal-title">Run Custom Query</h2>
      <div class="divider"></div>

      <textarea
        v-model="queryText"
        class="query-input"
        placeholder="Write your SQL query here…"
      ></textarea>

      <div class="modal-actions">
        <button type="button" class="btn btn-cancel" @click="closeModal">
          Cancel
        </button>
        <button type="button" class="btn btn-run" @click="runQuery">
          Run Query
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "QueryModal",
  data() {
    return {
      visible: false,
      queryText: "",
    };
  },
  methods: {
    showModal() {
      this.visible = true;
    },
    closeModal() {
      this.visible = false;
      this.queryText = "";
    },
    async runQuery() {
      if (!this.queryText.trim()) {
        alert("Query cannot be empty");
        return;
      }

      try {
        const instanceName = this.$cookies.get("selectedInstance");

        const response = await axios.post("http://localhost:50856/api/query", {
          query: this.queryText,
          instance: instanceName,
        });

        console.log("Query Response:", response.data);

        alert("Query executed successfully!");
        this.closeModal();
      } catch (err) {
        console.error(err);
        alert("Error running query. Check console.");
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
  backdrop-filter: blur(6px);
  background: rgba(0, 0, 0, 0.65);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 3000;
}

/* Modal Card */
.modal-box {
  width: 720px;
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
</style>
