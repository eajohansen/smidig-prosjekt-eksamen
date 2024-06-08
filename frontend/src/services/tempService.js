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
      birthdate: user.birthdate,
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
    if (result.status === 200) {
      console.log(result.data);
      localStorage.setItem("accessToken", result.data.accessToken);
      localStorage.setItem("refreshToken", result.data.refreshToken);
      axiosInstance.defaults.headers.common[
        "Authorization"
      ] = `Bearer ${localStorage.getItem("accessToken")}`;
    }
    return;
  } catch (err) {
    console.log(err);
    return err;
  }
};
export const sendEvent = async (event) => {
  console.log("what i get in:");
  console.log(event);
  try {
    const result = await axiosInstance.post("api/event/create", event);
    if (result.status === 200) {
      console.log("result: ");
      console.log(result);
    }
  } catch (err) {
    console.log(err);
  }
};

export const getEvents = async () => {
  try {
    const result = await axiosInstance.get("");
    return result;
  } catch (err) {
    console.log(err);
  }
};
