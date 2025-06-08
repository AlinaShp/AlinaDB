import axios from "axios";
const API_URL = "https://back.mahat.com/Authentication";

const getUser = async () => {
  try {
    const response = await axios.get(`${API_URL}`, {
      withCredentials: "true",
      headers: {
        "Content-Type": "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching user data:", error);
  }
};

export { getUser };
