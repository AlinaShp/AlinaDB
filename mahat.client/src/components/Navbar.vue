<template>
  <div id="dbComponent">
    <div class="custom-sidebar">
      <!-- Logo -->
      <div class="logo">ADB</div>

      <!-- Menu -->
      <div class="menu">
        <!-- HOME -->
        <a href="#" @click="logoff">
          <div class="icon">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="currentColor"
              viewBox="0 0 24 24"
              stroke-width="0.1"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M21.593 10.943c.584.585.584 1.53 0 2.116L18.71 15.95c-.39.39-1.03.39-1.42 0a.996.996 0 0 1 0-1.41 9.552 9.552 0 0 1 1.689-1.345l.387-.242-.207-.206a10 10 0 0 1-2.24.254H8.998a1 1 0 1 1 0-2h7.921a10 10 0 0 1 2.24.254l.207-.206-.386-.241a9.562 9.562 0 0 1-1.69-1.348.996.996 0 0 1 0-1.41c.39-.39 1.03-.39 1.42 0l2.883 2.893zM14 16a1 1 0 0 0-1 1v1.5a.5.5 0 0 1-.5.5h-7a.5.5 0 0 1-.5-.5v-13a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 .5.5v1.505a1 1 0 1 0 2 0V5.5A2.5 2.5 0 0 0 12.5 3h-7A2.5 2.5 0 0 0 3 5.5v13A2.5 2.5 0 0 0 5.5 21h7a2.5 2.5 0 0 0 2.5-2.5V17a1 1 0 0 0-1-1z"
              />
            </svg>
          </div>
          <div class="label">Logoff</div>
        </a>

        <!-- DATABASES -->
        <a href="#" @click="dbButtonClick">
          <div class="icon">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M12 3c4.418 0 8 .895 8 2v14c0 1.105-3.582 2-8 2s-8-.895-8-2V5c0-1.105 3.582-2 8-2z"
              />
            </svg>
          </div>
          <div class="label">Databases</div>
        </a>

        <!-- NEW DB -->
        <a href="#" @click="newDBButtonClick">
          <div class="icon">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor"
            >
              <path stroke-linecap="round" stroke-linejoin="round" d="M12 4v16m8-8H4" />
            </svg>
          </div>
          <div class="label">New DB</div>
        </a>

        <!-- NEW TABLE -->
        <a href="#" @click="newTableButtonClick">
          <div class="icon">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M4 6h16M4 10h16M4 14h16M4 18h16"
              />
            </svg>
          </div>
          <div class="label">New Table</div>
        </a>

        <!-- CUSTOM QUERY -->
        <a href="#" @click="customQueryButtonClick">
          <div class="icon">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor"
            >
              <path stroke-linecap="round" stroke-linejoin="round" d="M4.5 9.75l6 6m0-6l-6 6" />
            </svg>
          </div>
          <div class="label">Custom Query</div>
        </a>

        <!-- BACKUP -->
        <a href="#" @click="BackupQueryButtonClick">
          <div class="icon">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M12 16.5v-12m0 12l-4.5-4.5M12 16.5l4.5-4.5"
              />
            </svg>
          </div>
          <div class="label">Backup DB</div>
        </a>

        <!-- RESTORE -->
        <a href="#" @click="restoreButtonClick">
          <div class="icon">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M12 7.5v12m0-12l-4.5 4.5M12 7.5l4.5 4.5"
              />
            </svg>
          </div>
          <div class="label">Restore DB</div>
        </a>
      </div>
    </div>

    <!-- Modals -->
    <QueryModal ref="modalComponent" :instanceName="instanceName" />
    <BackupModal ref="backupModal" :instanceName="instanceName" />
    <RestoreModal ref="restoreModal" :instanceName="instanceName" />
    <NewDbModal ref="newdbModal" :instanceName="instanceName" />
    <NewTableModal ref="newtableModal" :instanceName="instanceName" />
  </div>
</template>

<script>
import QueryModal from "./QueryModal.vue";
import BackupModal from "./BackupModal.vue";
import RestoreModal from "./RestoreModal.vue";
import NewDbModal from "./NewDbModal.vue";

import NewTableModal from "./NewTableModal.vue";
import { mapActions } from "vuex";

export default {
  data() {
    return {
      instanceName: this.$cookies.get("selectedInstance"),
    };
  },
  components: { QueryModal, BackupModal, RestoreModal, NewDbModal, NewTableModal },
  methods: {
    async logoff() {
      await this.$store.dispatch("changeUser", "");
      this.$cookies.remove("selectedInstance");
      this.$router.push("/");
    },
    dbButtonClick() {
      this.$router.push("/DBview");
    },
    customQueryButtonClick() {
      this.$refs.modalComponent?.showModal();
    },
    BackupQueryButtonClick() {
      this.$refs.backupModal?.showModal();
    },
    restoreButtonClick() {
      this.$refs.restoreModal?.showModal();
    },
    newDBButtonClick() {
      this.$refs.newdbModal?.showModal();
    },
    newTableButtonClick() {
      this.$refs.newtableModal?.showModal();
    },
  },
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
  box-shadow: 0 0 22px rgba(255, 110, 200, 0.35);
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
  text-shadow: 0 0 10px rgba(255, 120, 200, 0.6);
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
  box-shadow: 0 0 16px rgba(255, 110, 200, 0.6);
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
