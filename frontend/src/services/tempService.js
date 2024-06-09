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
      allergies: allergyList,
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
      localStorage.setItem("loggedIn", "true");
      axiosInstance.defaults.headers.common[
        "Authorization"
      ] = `Bearer ${localStorage.getItem("accessToken")}`;
      const adminPrivileges = await axiosInstance.get("/api/user/checkAdminPrivileges");
        if (adminPrivileges.status === 200) {
          const adminRight = adminPrivileges.data.admin ? "true" : "false";
          const organizator = adminPrivileges.data.organizator ? "true" : "false";
            localStorage.setItem("admin", adminRight);
            localStorage.setItem("organizator", organizator);
        }
    }
    return;
  } catch (err) {
    console.log(err);
    return err;
  }
};
export const sendEvent = async (event) => {
  try {
    const result = await axiosInstance.post("api/event/create", {
      Event: {
        Title: event.title,
        OrganizationId: 8,
        Description: event.description,
        Published: event.published,
        Private: event.private,
        Place: {
          Location: event.address,
          Url: null,
        },
        Image: {
          Link: "test link",
          Description: "",
        },
        ContactPerson: {
          Name: event.contactP,
          Email: "test@test.no",
          Number: "",
        },
        EventCustomFields: event.EventCustomFields,
        Capacity: event.capacity,
        AgeLimit: event.ageLimit,
      },
      Start: event.start,
      StartTime: event.startTime,
      End: event.end,
      EndTime: event.endTime,
    });
    if (result?.status === 200) {
      console.log("result: ");
      console.log(result);
    }
    console.log(result);
  } catch (err) {
    console.log(err);
  }
};

export const getEvents = async () => {
  try {
    const result = await axiosInstance.get("api/event/fetchall");
    return result;
  } catch (err) {
    console.log(err);
  }
};
