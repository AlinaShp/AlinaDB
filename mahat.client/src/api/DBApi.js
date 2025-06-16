import axios from "axios";
const API_URL = "https://back.mahat.com/api/DB/";

const headers = {
  "Content-Type": "application/json",
  
};

const changeRecoveryModel = async (databaseName, instanceName) => {
  
  const response = await axios.patch(
    `${API_URL}changeRecoveryModel/${databaseName}?instanceName=${instanceName}`, {
    withCredentials: "true",
    headers: {
      "Content-Type": "application/json",
    },
  });

  return response;
};

const backupDatabase = async (databaseName , instanceName) => {
  
  const response = await axios.post(
    `${API_URL}backup/${databaseName}?instanceName=${instanceName}`, {
    withCredentials: "true",
    headers: {
      "Content-Type": "application/json",
    },
  });

  return response;
};

const restoreDatabase = async (databaseName, instanceName) => {
  
  const response = await axios.post(
    `${API_URL}restore/${databaseName}?instanceName=${instanceName}`, {
    withCredentials: "true",
    headers: {
      "Content-Type": "application/json",
    },
  });

  return response;
};

const getDBinfo = async (instanceName) => {
  

  const response = await axios.get(`${API_URL}DBdata?instanceName=${instanceName}`, {
    withCredentials: "true",
    headers: {
      "Content-Type": "application/json",
    },
  });

  return response;
};

const tableData = async (databaseName, tableName, instanceName) => {
  

  const response = await axios.get(`${API_URL}tableData/${databaseName}/${tableName}?instanceName=${instanceName}`, {
    withCredentials: "true",
    headers: {
      "Content-Type": "application/json",
    },
  });
  return response;
};

const tablesInfo = async (databaseName, instanceName) => {
  

  const response = await axios.get(`${API_URL}tablesInfo/${databaseName}?instanceName=${instanceName}`, {
    withCredentials: "true",
    headers: {
      "Content-Type": "application/json",
    },
  });
  return response;
}

export { changeRecoveryModel, backupDatabase, restoreDatabase, getDBinfo, tableData, tablesInfo };
