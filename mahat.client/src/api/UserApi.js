import axios from "axios";

const API_URL = "https://back.mahat.com/Authentication";

const getUser = async () => {
  const response = await axios.get(`${API_URL}`, {
    withCredentials: "true",
    headers: {
      "Content-Type": "application/json",
    },
  });
  return response;
};

export { getUser };
