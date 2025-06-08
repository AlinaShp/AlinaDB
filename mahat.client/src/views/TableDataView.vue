<template>
  <Navbar></Navbar>
</template>

<script>
import Navbar from "../components/Navbar.vue";
import { getTablesData } from "@/api/DBApi";
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
    };
  },
  methods: {
    async getTableinfo() {
      try {
        const instanceName = this.$globals.instanceName;
        const response = await getTablesData(this.dbName, instanceName);
        const res = response.data;
        console.log(res);
        this.tableCards = [...res];
      } catch (error) {
        console.error("There was an error!", error);
      }
    },
  },
  mounted() {
    this.getTableinfo();
  },
};
</script>

<style scoped></style>
