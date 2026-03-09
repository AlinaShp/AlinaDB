<template>
  <Navbar></Navbar>
  <h2 class="title">{{ dbName }}</h2>
  <div id="tableCards">
    <div class="row">
      <div class="col-4 mb-5" v-for="tableCard in tableCards" :key="tableCard.id">
        <TableCard :tableCard="tableCard" :databaseName="dbName"></TableCard>
      </div>
    </div>
  </div>
</template>

<script>
import TableCard from "../components/TableCard.vue";
import Navbar from "../components/Navbar.vue";
import { tablesInfo } from "@/api/DBApi";
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
    async tablesInfo() {
      try {
        //TEST
        const instanceName = this.$cookies.get('selectedInstance')
        //const response = await tablesInfo(this.dbName, instanceName);
        //const res = response.data;
       // this.tableCards = [...res];
        const tableCards_response = [
      {
        TableName: "Employees",
        PrimaryKey: "EmployeeID"
      },
      {
        TableName: "Orders",
        PrimaryKey: "OrderID"
      },
      {
        TableName: "Products",
        PrimaryKey: "ProductID"
      },
      {
        TableName: "Customers",
        PrimaryKey: "CustomerID"
      },
      {
        TableName: "Departments",
        PrimaryKey: "DepartmentID"
      },
      {
        TableName: "AuditLog",
        PrimaryKey: null 
      }
    ];

      this.tableCards = tableCards_response;
      } catch (error) {
        console.error("There was an error!", error);
      }
    },
  },
    mounted() { 
    this.tablesInfo();
  },
};
</script>

<style scoped>
#tableCards {
  width: 100%;
  display: flex;
  justify-content: center; /* centers the row */
  margin-top: 20px;
  padding-left: 20vh; 
}
</style>
