<template>  
  <div id="dbComponent">

    <!-- Smooth blurred background -->
    <div id="blur-background"></div>

    <Navbar />

    <h2 class="title">{{ this.$cookies.get('selectedInstance') }}</h2>

    <!-- Responsive grid container -->
    <div id="cards-grid">
      <DBcard 
        v-for="card in cards"
        :key="card.DatabaseId"
        :card="card"
        @delete="confirmDeleteDb"
        @buttonClick="tableButtonClick(card.DatabaseName)"
      />
    </div>

    <!-- Modals -->
    <QueryModal ref="modalComponent" />
    <BackupModal ref="backupModalComponent" />
    <RestoreModal ref="restoreModalComponent" />

  </div>
</template>

<script>
import Navbar from '../components/Navbar.vue';
import DBcard from "../components/DBcard.vue";
import QueryModal from "../components/QueryModal.vue";
import BackupModal from "../components/BackupModal.vue";
import RestoreModal from "../components/RestoreModal.vue";

export default {
  components: {
    Navbar,
    DBcard,
    QueryModal,
    BackupModal,
    RestoreModal
  },
  data(){
    return{
      cards:[],
    };
  },
  methods:{
    tableButtonClick(dbName) {
      this.$router.push({ path: '/DBview/TablesView', query: { dbName: dbName } });
    },
    confirmDeleteDb(db) {
  if (confirm(`Delete database "${db.DatabaseName}"?`)) {
    alert("Do you want to delete?");
  }
},

    async getDBinfo(){
      // TEST ONLY
      this.cards = [
        { DatabaseName: "AdventureWorks2022", DatabaseId: 1, RecoveryModel: "Full", State: "Online", CompatibilityLevel: 150, Collation: "SQL_Latin1_General_CP1_CI_AS" },
        { DatabaseName: "master", DatabaseId: 2, RecoveryModel: "Simple", State: "Online", CompatibilityLevel: 160, Collation: "SQL_Latin1_General_CP1_CI_AS" },
        { DatabaseName: "ReportDB", DatabaseId: 3, RecoveryModel: "BulkLogged", State: "Restoring", CompatibilityLevel: 140, Collation: "Hebrew_CI_AS" },
        { DatabaseName: "testDB", DatabaseId: 4, RecoveryModel: "Full", State: "Online", CompatibilityLevel: 150, Collation: "SQL_Latin1_General_CP1_CI_AS" },
        { DatabaseName: "AnalyticsDB", DatabaseId: 5, RecoveryModel: "Simple", State: "Online", CompatibilityLevel: 160, Collation: "SQL_Latin1_General_CP1_CI_AS" }
      ];
    }
  },
  mounted(){
    this.getDBinfo();
  }
}
</script>

<style>

/* GLOBAL RESET */
html, body, #app, #dbComponent {
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
  background-image: url('https://img.freepik.com/free-vector/abstract-geometric-round-shape-blue-background-design_1017-42785.jpg');
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
  margin-left: 14vh;   /* space for navbar */
  margin-top: 40px;
  margin-bottom: 40px;

  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));

  gap: 32px;   /* perfect spacing between all rows & columns */

  padding: 0 40px;   /* side padding */
}

/* MODAL OVERLAY FIX */
.modal-overlay {
  position: fixed !important;
  inset: 0;
  backdrop-filter: blur(5px);
  background: rgba(0,0,0,0.55);
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
  from { opacity: 0; transform: translateY(-10px); }
  to { opacity: 1; transform: translateY(0); }
}

</style>
