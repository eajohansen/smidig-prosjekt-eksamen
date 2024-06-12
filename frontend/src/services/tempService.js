import { axiosInstance } from "./helpers";
import { useNavigate } from "react-router-dom";

export const sendRegister = async (email, password) => {
  try {
    const result = await axiosInstance.post("register", { email, password });
    //[PUT /api/user/update
    if (result.status === 200) {
      const res = await sendLogin(email, password);
      if (res.status === 200) {
        //return email
      }
      console.log(res.status);
      return result.status === 200;
    }
  } catch (err) {
    console.log(err);
    return err;
  }
};
export const sendUser = async (user, allergyList) => {
  // const navigate = useNavigate();
  try {
    console.log(user, allergyList);
    const result = await axiosInstance.put("api/user/update", {
      email: user.email,
      firstname: user.firstName,
      lastname: user.lastName,
      birthdate: user.birthdate,
      allergies: allergyList,
    });
    if (result.status === 200) {
      return result.status === 200;
    }
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
      const adminPrivileges = await axiosInstance.get(
        "/api/user/checkAdminPrivileges"
      );
      if (adminPrivileges.status === 200) {
        const adminRight = adminPrivileges.data.admin ? "true" : "false";
        const organizator = adminPrivileges.data.organizator ? "true" : "false";
        localStorage.setItem("admin", adminRight);
        localStorage.setItem("organizator", organizator);
      }
    }
    return result.status === 200;
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
        OrganizationId: 1,
        Description: event.description,
        Published: event.published,
        Private: event.private,
        Place: {
          Location: event.address,
          URL: null,
        },
        Image: {
          Link: "test link",
          Description: "",
        },
        contactPerson: {
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
      return result;
    }
    console.log(result);
  } catch (err) {
    console.log(err);
  }
};

export const getEvents = async () => {
  try {
    const result = await axiosInstance.get("api/event/fetchall");
    console.log(result);
    return result.data;
  } catch (err) {
    console.log(err);
  }
};

export const getEventById = async (id) => {
  try {
    const result = await axiosInstance.get(`api/event/fetch/id/${id}`);
    // `GET /api/event/fetch/id/{eventId}`
    console.log(result);
    return result?.data;
  } catch (err) {
    console.log(err);
  }
};

export const sendOrg = async (name, description) => {
  try {
    const result = await axiosInstance.post("api/organization/create", {
      name,
      description,
    });
    return result;
  } catch (err) {
    console.log(err);
  }
};

export const fetchOrg = async () => {
  try {
    const result = await axiosInstance.get("api/organization/fetch/id/1");
    if (result?.status === 200) {
      return result;
    } else {
      return null;
    }
  } catch (err) {
    console.log(err);
  }
};

export const editOrg = async (orgInfo) => {
  try {
    const result = await axiosInstance.put("api/organization/update", {
      OrganizationId: 1,
      Name: orgInfo.name,
      Description: orgInfo.description,
    });
    return result;
  } catch (err) {
    console.log(err);
  }
};
export const isAdmin = async () => {
  try {
    const result = await axiosInstance.get("api/user/checkAdminPrivileges");
    return result;
  } catch (err) {
    console.log(err);
  }
};
