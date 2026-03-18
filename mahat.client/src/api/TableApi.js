import axios from "axios";
const API_URL = "https://back.mahat.com/api/table/";
//const API_URL = "http://localhost:5283/api/TableController/";

const addTable = async (databaseName, instanceName, payload) => {
  const response = await axios.post(
    `${API_URL}addTable/${databaseName}?instancename=${instanceName}`,
    payload,
    {
      withCredentials: true,
      headers: { "Content-Type": "application/json" },
    },
  );
  return response;
};

const deleteTable = async (databaseName, tableName, instanceName) => {
  const response = await axios.delete(
    `${API_URL}dropTable/${databaseName}/${tableName}?instanceName=${instanceName}`,
    {
      withCredentials: true,
      headers: { "Content-Type": "application/json" },
    },
  );
  return response;
};

const insertRow = async (databaseName, tableName, instanceName, rowData) => {
  const response = await axios.post(
    `${API_URL}insertRow/${databaseName}/${tableName}?instanceName=${instanceName}`,
    rowData,
    {
      withCredentials: true,
      headers: { "Content-Type": "application/json" },
    },
  );
  return response;
};

const updateRow = async (
  databaseName,
  tableName,
  instanceName,
  primaryKeyName,
  primaryKeyValue,
  rowData,
) => {
  const response = await axios.patch(
    `${API_URL}updateRow/${databaseName}/${tableName}/${primaryKeyName}/${primaryKeyValue}?instanceName=${instanceName}`,
    rowData,
    {
      withCredentials: true,
      headers: { "Content-Type": "application/json" },
    },
  );
  return response;
};

const deleteRow = async (
  databaseName,
  tableName,
  instanceName,
  primaryKeyName,
  primaryKeyValue,
) => {
  const response = await axios.delete(
    `${API_URL}deleteRow/${databaseName}/${tableName}/${primaryKeyName}/${primaryKeyValue}?instanceName=${instanceName}`,
    {
      withCredentials: true,
      headers: { "Content-Type": "application/json" },
    },
  );
  return response;
};

const getTableData = async (databaseName, tableName, instanceName) => {
  const response = await axios.get(
    `${API_URL}tableData/${databaseName}/${tableName}?instanceName=${instanceName}`,
    {
      withCredentials: true,
      headers: { "Content-Type": "application/json" },
    },
  );
  return response;
};

const getTableColumns = async (databaseName, tableName, instanceName) => {
  const response = await axios.get(
    `${API_URL}tableColumns/${databaseName}/${tableName}?instanceName=${instanceName}`,
    {
      withCredentials: true,
      headers: { "Content-Type": "application/json" },
    },
  );
  return response;
};

const tablesInfo = async (databaseName, instanceName) => {
  const response = await axios.get(
    `${API_URL}tablesInfo/${databaseName}?instancename=${instanceName}`,
    {
      withCredentials: "true",
      headers: {
        "Content-Type": "application/json",
      },
    },
  );
  return response;
};

export {
  addTable,
  deleteTable,
  insertRow,
  updateRow,
  deleteRow,
  getTableData,
  getTableColumns,
  tablesInfo,
};
