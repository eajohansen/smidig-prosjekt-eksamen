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
export const sendUser = async (user) => {
  try {
    console.log(user);
    const result = await axiosInstance.post("api/user/create", {
      email: user.email,
      firstname: user.firstName,
      lastname: user.lastName,
    });
    return result.status === 200;
  } catch (err) {
    console.log(err);
    return err;
  }
};
