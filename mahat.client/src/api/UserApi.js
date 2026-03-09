import axios from "axios";
const API_URL = " https://localhost:7239/Authentication";
//const API_URL = "https://WIN-6OA5JFTRURC.mahat.com:5283/Authentication";

/*const agent = new https.Agent({  
  rejectUnauthorized: false
});*/

const getUser = async () => {
  try {
    const response = await axios.get(`${API_URL}`, {
      withCredentials: "true",
      httpsAgent: agent,
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
