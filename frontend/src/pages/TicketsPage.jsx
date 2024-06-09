import React from "react";
import { axiosInstance } from "../services/helpers";
export const TicketsPage = () => {
  const testClick = async () => {
    const result = await axiosInstance.get("api/user/fetchall");
    console.log(result);
  };
  return (
    <div>
      {" "}
      <button className="testBtn" onClick={testClick}>
        Klikk meg
      </button>
    </div>
  );
};
