import axios from "axios";

export const axiosInstance = axios.create({
  baseURL: "http://localhost:5500/",
  withCredentials: true,
  headers: {
    "Bearer ": "",
  },
});

axiosInstance.interceptors.response.use(
  async (response) => {
    return response;
  },
  async (error) => {
    try {
      const originalConfig = error?.config;

      if (error.response.status === 401 && !originalConfig._retry) {
        const resp = await axios.post("http://localhost:5500/refresh", {
          refreshToken: localStorage.getItem("refreshToken"),
        });

        if (resp.status === 200) {
          localStorage.setItem("accessToken", resp.data.accessToken);
          localStorage.setItem("refreshToken", resp.data.refreshToken);
          axiosInstance.defaults.headers.common[
            "Authorization"
          ] = `Bearer ${localStorage.getItem("accessToken")}`;
          originalConfig._retry = true;
          return await axiosInstance.request(error.config);
        }
      } else if (error?.response?.status === 401) {
        localStorage.clear();
        // router.navigate("/") eller
        // todo log out routing
        return Promise.reject(error);
      }
    } catch {
      // router.navigate("/")
      return Promise.reject(error);
    }
  }
);
