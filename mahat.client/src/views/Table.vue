<template>
  <Navbar></Navbar>
  <div id="cards">
    <h1>table</h1>
    <table class="table">
      <thead>
        <tr>
          <th v-for="(header, index) in tableHeaders" :key="index" scope="col">{{ header }}</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(row, index) in tableData" :key="index">
          <td v-for="(header, index) in tableHeaders" :key="index">{{ row[header] }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import Navbar from "../components/Navbar.vue";
import { getTableInfo } from "@/api/DBApi";
export default {
  components: {
    Navbar,
  },
  props: {
    databaseName: {
      type: String,
      required: true,
    },
    tableName: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      tableData: [],
      tableHeaders: [],
    };
  },
  methods: {
    async getTablesInfo() {
      try {
        const instanceName = this.$globals.instanceName;
        const respponse = await getTableInfo(this.databaseName, this.tableName, instanceName);

        console.log(response.data);
        this.tableData = response.data;
        if (this.tableData.length > 0) {
          this.tableHeaders = Object.keys(this.tableData[0]);
        }
      } catch (error) {
        console.error("There was an error!", error);
      }
    },
    mounted() {
      this.getTablesInfo();
    },
  },
};
</script>
<style></style>
