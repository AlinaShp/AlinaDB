<template>
  <div id="dbComponent">
    <div id="blur-background"></div>

    <Navbar />

    <h2 class="title">{{ this.$cookies.get("selectedInstance") }}</h2>

    <div id="cards-grid">
      <DBcard
        v-for="card in cards"
        :key="card.DatabaseId"
        :card="card"
        @delete="confirmDeleteDb"
        @buttonClick="tableButtonClick"
      />
    </div>

    <QueryModal ref="modalComponent" />
    <BackupModal ref="backupModalComponent" />
    <RestoreModal ref="restoreModalComponent" />
  </div>
</template>

<script>
import Navbar from "../components/Navbar.vue";
import DBcard from "../components/DBcard.vue";
import QueryModal from "../components/QueryModal.vue";
import BackupModal from "../components/BackupModal.vue";
import RestoreModal from "../components/RestoreModal.vue";
import Swal from "sweetalert2";

import { getDBinfo, deleteDatabase } from "../api/DBApi";

export default {
  components: {
    Navbar,
    DBcard,
    QueryModal,
    BackupModal,
    RestoreModal,
  },

  data() {
    return {
      cards: [],
    };
  },

  methods: {
    tableButtonClick(dbName) {
      this.$router.push({
        path: "/DBview/TablesView",
        query: { dbName: dbName },
      });
    },

    async confirmDeleteDb(db) {
      const instanceName = this.$cookies.get("selectedInstance");
      try {
        Swal.fire({
          title: "Deleting database...",
          allowOutsideClick: false,
          allowEscapeKey: false,
          didOpen: () => {
            Swal.showLoading();
          },
        });

        const response = await deleteDatabase(db.databaseName, instanceName);

        Swal.fire({
          icon: "success",
          title: "Database Deleted",
          text: response.data || "Database deleted successfully",
          timer: 1500,
          showConfirmButton: false,
        });

        await this.getDBinfo();
      } catch (error) {
        console.error(error);

        const msg = error.response?.data?.message || error.message;

        Swal.fire({
          icon: "error",
          title: "Database Delete Failed",
          text: msg,
        });
      }
    },

    async getDBinfo() {

      try {
        const instanceName = this.$cookies.get("selectedInstance");
        Swal.fire({
          title: "Loading databases...",
          allowOutsideClick: false,
          allowEscapeKey: false,
          didOpen: () => {
            Swal.showLoading();
          },
        });
        const response = await getDBinfo(instanceName);

        this.cards = response.data;
        Swal.close();
      } catch (error) {
        console.error(error);

        const msg = error.response?.data?.message || error.message;
        Swal.fire({
          icon: "error",
          title: "Failed to load databases",
          text: msg,
        });
      }
    },
  },

  mounted() {
    this.getDBinfo();
  },
};
</script>

<style>
/* GLOBAL RESET */
html,
body,
#app,
#dbComponent {
  margin: 0;
  padding: 0;

  overflow-x: hidden;
}

/* BEAUTIFUL BLURRED BACKGROUND */
#blur-background {
  position: fixed;
  top: -20px;
  left: -20px;
  width: calc(100vw + 40px);
  height: calc(100vh + 40px);
  background-image: url("https://img.freepik.com/free-vector/abstract-geometric-round-shape-blue-background-design_1017-42785.jpg");
  background-size: cover;
  background-position: center;
  filter: blur(15px);
  z-index: -1;
}

/* PAGE TITLE */
.title {
  margin-top: 4vh;
  font-family: "Rubik", sans-serif;
  font-weight: 700;
  font-size: 3rem; /* responsive instead of fixed 7em */
  color: pink;
  margin: 0 auto 25px auto; /* ensures perfect horizontal centering */
  text-align: center;
  animation: fadeInTitle 0.8s ease-out;

  text-shadow: 0 0 18px rgba(255, 140, 200, 0.6);
}

/* UPDATED RESPONSIVE GRID */
#cards-grid {
  margin-left: 14vh; /* space for navbar */
  margin-top: 40px;
  margin-bottom: 40px;

  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));

  gap: 32px; /* perfect spacing between all rows & columns */

  padding: 0 40px; /* side padding */
}

/* MODAL OVERLAY FIX */
.modal-overlay {
  position: fixed !important;
  inset: 0;
  backdrop-filter: blur(5px);
  background: rgba(0, 0, 0, 0.55);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 3000;
}

.modal-backdrop {
  display: none !important;
}

/* Title styling */
h2 {
  font-size: 2.4rem;
  font-weight: 700;
  color: #ffffff;
  text-align: center;

  /* soft glowing effect */
  text-shadow:
    0 0 6px rgba(255, 255, 255, 0.45),
    0 0 12px rgba(0, 153, 255, 0.5);
}

@keyframes fadeInTitle {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>
