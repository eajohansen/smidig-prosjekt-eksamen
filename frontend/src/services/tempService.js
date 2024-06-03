import {axiosInstance} from "../helper/axios.js";

export const sendRegister = async (email, password) => {
  try {
   await axiosInstance.post("register",
       { email, password }
   );
    return "Success!"
  } catch (err) {
    console.log(err.response.data.errors)
    return err.response.data.errors;
  }
};

