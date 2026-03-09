import axios from "axios";
//const API_URL = "https://back.mahat.com/api/DB/";.
const API_URL = "http://localhost:5283/api/DBcontroller/";

//not used
const changeRecoveryModel = async (databaseName, instanceName) => {
  const response = await axios.patch(
    `${API_URL}changeRecoveryModel/${databaseName}?instancename=${instanceName}`,
    {
      withCredentials: "true",
      headers: {
        "Content-Type": "application/json",
      },
    },
  );

  return response;
};

//new request - Add new DB
const createDatabase = async (instanceName) => {
  const response = await axios.post(
    `${API_URL}addDB/${instanceName}`,
    {
      withCredentials: "true",
      headers: {
        "Content-Type": "application/json",
      },
      ...payload,
    },
  );
  return response;
};

//new request - Delete Database
const deleteDatabase = async (databaseName, instanceName) => {
  const response = await axios.delete(
    `${API_URL}deleteDB/${databaseName}?instanceName=${instanceName}`,
    {
      withCredentials: true,
      headers: {
        "Content-Type": "application/json",
      },
    }
  );

  return response;
};

//new request - Execute query
const executeQuery = async (instanceName, queryText) => {
  const response = await axios.post(`${API_URL}DBdata?instancename=${instanceName}`,
    queryText, {
    withCredentials: "true",
    headers: {
      "Content-Type": "application/json",
    },

  });

  return response;
};

//Add backup type, backup file path, backup filename
const backupDatabase = async (databaseName, instanceName) => {
  const response = await axios.post(
    `${API_URL}backup/${databaseName}?instancename=${instanceName}`,
    {
      withCredentials: "true",
      headers: {
        "Content-Type": "application/json",
      },
    },
  );

  return response;
};

//Add backup filepath, overwrite existing db
const restoreDatabase = async (databaseName, instanceName) => {
  const response = await axios.post(
    `${API_URL}restore/${databaseName}?instancename=${instanceName}`,
    {
      withCredentials: "true",
      headers: {
        "Content-Type": "application/json",
      },
    },
  );

  return response;
};

const getDBinfo = async (instanceName) => {
  const response = await axios.get(
    `${API_URL}DBdata?instancename=${instanceName}`,
    {
      withCredentials: true,
      headers: {
        "Content-Type": "application/json",
      },
    }
  );

  return response;
};

const tableData = async (databaseName, tableName, instanceName) => {
  const response = await axios.get(
    `${API_URL}tableData/${databaseName}/${tableName}?instancename=${instanceName}`,
    {
      withCredentials: "true",
      headers: {
        "Content-Type": "application/json",
      },
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

export { createDatabase, deleteDatabase, changeRecoveryModel, backupDatabase, restoreDatabase, getDBinfo, tableData, tablesInfo, executeQuery };
