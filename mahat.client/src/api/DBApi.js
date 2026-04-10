import axios from "axios";
const API_URL = "https://back.mahat.com/api/DB/";

const checkConnection = async (instanceName) => {
  const response = await axios.get(
    `${API_URL}checkConnection?instancename=${instanceName}`,
    {
      withCredentials: "true",
      headers: {
        "Content-Type": "application/json",
      },
    },
  );

  return response
}

//new request - Add new DB
const createDatabase = async (instanceName, payload) => {
  const response = await axios.post(`${API_URL}addDB/?instancename=${instanceName}`, payload, {
    withCredentials: "true",
    headers: {
      "Content-Type": "application/json",
    },
  });
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
    },
  );

  return response;
};

//new request - Execute query
const executeQuery = async (instanceName, queryText) => {
  const response = await axios.post(
    `${API_URL}executeQuery?instancename=${instanceName}`,
    queryText,
    {
      withCredentials: "true",
      headers: {
        "Content-Type": "application/json",
      },
    },
  );

  return response;
};

//Add backup type, backup file path, backup filename
const backupDatabase = async (databaseName, instanceName, payload) => {
  const response = await axios.post(
    `${API_URL}backup/${databaseName}?instancename=${instanceName}`,
    payload,
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
const restoreDatabase = async (databaseName, instanceName, payload) => {
  const response = await axios.post(
    `${API_URL}restore/${databaseName}?instancename=${instanceName}`,
    payload,
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
  const response = await axios.get(`${API_URL}DBinfo?instancename=${instanceName}`, {
    withCredentials: true,
    headers: {
      "Content-Type": "application/json",
    },
  });

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

const exsitingBackups = async (databaseName, instanceName, backupType) => {
  const response = await axios.get(
    `${API_URL}existingBackups/${databaseName}/${backupType}?instancename=${instanceName}`,
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
  checkConnection,
  createDatabase,
  deleteDatabase,
  changeRecoveryModel,
  backupDatabase,
  restoreDatabase,
  getDBinfo,
  tableData,
  executeQuery,
  exsitingBackups
};
