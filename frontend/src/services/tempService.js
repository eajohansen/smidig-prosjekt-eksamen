import { axiosInstance } from "./helpers";

export const sendRegister = async (email, password) => {
  try {
    const result = await axiosInstance.post("register", { email, password });
    return result.status === 200;
  } catch (err) {
    console.log(err.response.data.errors);
    return err.response.data.errors;
  }
};
export const sendUser = async (user, allergyList) => {
  try {
    console.log(user, allergyList);
    const result = await axiosInstance.post("api/user/create", {
      email: user.email,
      firstname: user.firstName,
      lastname: user.lastName,
      allergyList,
    });
    return result.status === 200;
  } catch (err) {
    console.log(err);
    return err;
  }
};
export const sendLogin = async (email, password) => {
  try {
    const result = await axiosInstance.post("login", { email, password });
    return result.status === 200;
  } catch (err) {
    console.log(err);
    return err;
  }
};
