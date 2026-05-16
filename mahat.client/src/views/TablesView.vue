<template>
  <Navbar></Navbar>

  <h2 class="title">{{ dbName }}</h2>

  <div id="tableCards">
    <div class="row">
      <div class="col-4 mb-5" v-for="tableCard in tableCards" :key="tableCard.TableName">
        <TableCard :tableCard="tableCard" :databaseName="dbName" @deleteTable="deleteTable" />
      </div>
    </div>
  </div>
</template>

<script>
import TableCard from "../components/TableCard.vue";
import Navbar from "../components/Navbar.vue";
import Swal from "sweetalert2";
import { deleteTable, tablesInfo } from "@/api/TableApi";

export default {
  components: {
    TableCard,
    Navbar,
  },

  props: {
    dbName: {
      type: String,
      required: true,
    },
  },

  data() {
    return {
      tableCards: [],
      databaseName: this.dbName,
    };
  },

  methods: {
    async loadTables() {
      const instanceName = this.$cookies.get("selectedInstance");
      try {
        Swal.fire({
          title: "Loading tables...",
          allowOutsideClick: false,
          allowEscapeKey: false,
          didOpen: () => {
            Swal.showLoading();
          },
        });
        const response = await tablesInfo(this.dbName, instanceName);

        this.tableCards = response.data;

        Swal.close();
      } catch (error) {
        console.error(error);

        const msg = error.response?.data?.message || error.message;

        Swal.fire({
          icon: "error",
          title: "Failed to load tables",
          text: msg,
        });
      }
    },
    async deleteTable(tableName) {
      debugger;
      const instanceName = this.$cookies.get("selectedInstance");

      try {
        Swal.fire({
          title: "Deleting table...",
          allowOutsideClick: false,
          allowEscapeKey: false,
          didOpen: () => {
            Swal.showLoading();
          },
        });

        const response = await deleteTable(this.dbName, tableName, instanceName);

        Swal.fire({
          icon: "success",
          title: "Table Deleted",
          text: response.data || "Table dropped successfully",
          timer: 1500,
          showConfirmButton: false,
        });

        // reload tables
        await this.loadTables();
      } catch (error) {
        console.error(error);

        const msg = error.response?.data?.message || error.message;

        Swal.fire({
          icon: "error",
          title: "Table Delete Failed",
          text: msg,
        });
      }
    },
  },

  mounted() {
    this.loadTables();
  },
};
</script>

<style scoped>
#tableCards {
  width: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  padding-left: 20vh;
}
.col-4 {
  flex: 0 0 300px; /* Fixed width for each card */
  height: 200px; /* Fixed height for each card */
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>
