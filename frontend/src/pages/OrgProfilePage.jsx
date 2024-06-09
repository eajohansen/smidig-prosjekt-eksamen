import React from "react";
import { MainNav } from "../components/shared/MainNav";
import { useState, useEffect } from "react";
import "bootstrap-icons/font/bootstrap-icons.css";
import { sendOrg } from "../services/tempService";

export const OrgProfilePage = () => {
  const [orgInfo, setOrgInfo] = useState({ name: "", description: "" });

  const handleChange = (e) => {
    setOrgInfo({ ...orgInfo, [e.currentTarget.name]: e.currentTarget.value });
  };

  const handleSubmit = async () => {
    console.log(orgInfo);
    if (orgInfo.name != "") {
      const result = await sendOrg(orgInfo.name, orgInfo.description);
      console.log(result);
    }
  };
  return (
    <>
      <div className="OrgProfilContainer">
        <div className="headderContainer">
          <h2>Organisasjon Profil</h2>
          <hr></hr>
        </div>

        <div className="profileInfoContainer inputHover">
          <div className="userInfoContainer">
            <label htmlFor="emailRegister">Epost</label>
            <div className="emailContainer">
              <input type="email" id="emailRegister" value="epost" readOnly />

              <span className="icon-inside">
                <i className="bi bi-lock"></i>
              </span>
            </div>
            <label htmlFor="nameRegister">Organization Name</label>
            <input
              type="text"
              id="nameRegister"
              name="name"
              onChange={handleChange}
            />
            <a className="chageEMOrg" href="">
              Endre E-post
            </a>
            <a href="">Endre passord</a>
          </div>

          <div className="aboutUsContainer">
            <label htmlFor="AboutUs">Om oss</label>
            <input
              type="text"
              id="infoAboutUs"
              name="description"
              onChange={handleChange}
            />
            <div className="imgContainer disclaimerDiv">
              <p>Last opp bilde:</p>
              <button className="chooseBtn">Velg bilde</button>
            </div>
            <div className="btnDiv">
              <button className="canclBtn">Avbryt</button>
              <button className="createBtn" onClick={handleSubmit}>
                Opprett Organisasjon
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
