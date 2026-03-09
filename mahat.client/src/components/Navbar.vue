<template>
  <div id="dbComponent">
    <div class="custom-sidebar">

      <!-- Logo -->
      <div class="logo">ADB</div>

      <!-- Menu -->
      <div class="menu">

        <!-- HOME -->
        <a href="#" @click="goHome">
          <div class="icon">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none"
                 viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round"
                    d="M3 10.5L12 3l9 7.5V21a.75.75 0 01-.75.75h-5.25V15H9v6.75H3.75A.75.75 0 013 21v-10.5z" />
            </svg>
          </div>
          <div class="label">Home</div>
        </a>

        <!-- DATABASES -->
        <a href="#" @click="dbButtonClick">
          <div class="icon">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none"
                 viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round"
                    d="M12 3c4.418 0 8 .895 8 2v14c0 1.105-3.582 2-8 2s-8-.895-8-2V5c0-1.105 3.582-2 8-2z" />
            </svg>
          </div>
          <div class="label">Databases</div>
        </a>

        <!-- NEW DB -->
        <a href="#" @click="newDBButtonClick">
          <div class="icon">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none"
                 viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round"
                    d="M12 4v16m8-8H4" />
            </svg>
          </div>
          <div class="label">New DB</div>
        </a>

        <!-- NEW TABLE -->
        <a href="#" @click="newTableButtonClick">
          <div class="icon">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none"
                 viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round"
                    d="M4 6h16M4 10h16M4 14h16M4 18h16" />
            </svg>
          </div>
          <div class="label">New Table</div>
        </a>

        <!-- CUSTOM QUERY -->
        <a href="#" @click="customQueryButtonClick">
          <div class="icon">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none"
                 viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round"
                    d="M4.5 9.75l6 6m0-6l-6 6" />
            </svg>
          </div>
          <div class="label">Custom Query</div>
        </a>

        <!-- BACKUP -->
        <a href="#" @click="BackupQueryButtonClick">
          <div class="icon">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none"
                 viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round"
                    d="M12 16.5v-12m0 12l-4.5-4.5M12 16.5l4.5-4.5" />
            </svg>
          </div>
          <div class="label">Backup DB</div>
        </a>

        <!-- RESTORE -->
        <a href="#" @click="restoreButtonClick">
          <div class="icon">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none"
                 viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round"
                    d="M12 7.5v12m0-12l-4.5 4.5M12 7.5l4.5 4.5" />
            </svg>
          </div>
          <div class="label">Restore DB</div>
        </a>
      </div>
    </div>

    <!-- Modals -->
    <QueryModal ref="modalComponent" />
    <BackupModal ref="backupModal" />
    <RestoreModal ref="restoreModal" />
    <NewDbModal ref="newdbModal" />
    <NewTableModal ref="newtableModal" />
    
  </div>
</template>

<script>
import QueryModal from "./QueryModal.vue";
import BackupModal from "./BackupModal.vue";
import RestoreModal from "./RestoreModal.vue";
import NewDbModal from "./NewDbModal.vue";
import NewTableModal from "./NewTableModal.vue";


export default {
  components: { QueryModal, BackupModal, RestoreModal, NewDbModal, NewTableModal },
  methods: {
    goHome() { this.$router.push("/"); },
    dbButtonClick() { this.$router.push("/DBview"); },
    customQueryButtonClick() { this.$refs.modalComponent?.showModal(); },
    BackupQueryButtonClick() { this.$refs.backupModal?.showModal(); },
    restoreButtonClick() { this.$refs.restoreModal?.showModal(); },
    newDBButtonClick() { this.$refs.newdbModal?.showModal(); },
    newTableButtonClick() { this.$refs.newtableModal?.showModal(); },
  }
};
</script>

<style scoped>
@import url("https://fonts.googleapis.com/css2?family=Nunito:wght@400;600;700&display=swap");

/* Sidebar container */
.custom-sidebar {
  position: fixed;
  top: 24px;
  left: 24px;
  width: 90px; /* collapsed width */
  height: calc(100vh - 48px);
  background: rgba(45, 55, 90, 0.6);
  backdrop-filter: blur(14px);
  border-radius: 18px;
  padding: 20px 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  transition: width 0.35s ease;
  box-shadow: 0 0 22px rgba(255,110,200,0.35);
  z-index: 100;
  overflow: hidden;
  font-family: "Nunito", sans-serif;
}

/* Expand on hover */
.custom-sidebar:hover {
  width: 50vh;
}

/* Logo */
.logo {
  font-size: 1.8em;
  font-weight: 900;
  color: hotpink;
  margin-bottom: 24px;
  text-shadow: 0 0 10px rgba(255,120,200,0.6);
}

/* Menu */
.menu {
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 12px;
  padding: 0;
}

/* Menu item */
.menu a {
  width: 38vh;
  height: 48px;
  display: flex;
  align-items: center;
  gap: 16px;
  border-radius: 14px;
  text-decoration: none;
  color: #f3f3f3;
  transition: 0.25s ease;
  padding: 0; /* no padding when collapsed */
 margin-left: 5vh;
 margin-right: 5vh;
}

/* Hover effect */
.menu a:hover {
  background: linear-gradient(90deg, #ff6ec7, #ff9ce6);
  color: #2d1f2c;
  box-shadow: 0 0 16px rgba(255,110,200,0.6);
  transform: translateX(6px);
  
}

/* When sidebar is hovered, align icon + label properly */
.custom-sidebar:hover .menu a {
  justify-content: flex-start; /* align left */
  padding: 0 16px; /* restore horizontal padding */
}

/* Icon container */
.icon {
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.icon svg {
  width: 26px;
  height: 26px;
  transition: transform 0.25s ease;
}

.menu a:hover .icon svg {
  transform: scale(1.15);
}

/* Labels */
.label {
  opacity: 0;
  white-space: nowrap;
  font-weight: 700;
  transition: opacity 0.3s ease;
}

.custom-sidebar:hover .label {
  opacity: 1;
}

/* Responsive */
@media (max-width: 600px) {
  .custom-sidebar {
    width: 300px;
    height: auto;
    left: 0;
    transform: translateX(-100%);
  }
  .custom-sidebar.active {
    transform: translateX(0);
  }
  .custom-sidebar:hover {
    width: 300px;
  }
}


</style>
