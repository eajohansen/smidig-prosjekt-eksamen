import React from "react";
import { useState, useEffect } from "react";
import "bootstrap-icons/font/bootstrap-icons.css";
import { fetchOrg, sendOrg, editOrg } from "../services/tempService";
import {useNavigate} from "react-router-dom";

export const OrgProfilePage = () => {
  const navigate = useNavigate();
  const [orgInfo, setOrgInfo] = useState({ name: "", description: "" });
  const [alreadyOrg, setAlreadyOrg] = useState(false);
  const userEmail = localStorage.getItem("emailStore");
  const handleChange = (e) => {
    setOrgInfo({ ...orgInfo, [e.currentTarget.name]: e.currentTarget.value });
  };

  const handleSubmit = async () => {
    console.log(orgInfo);
    if (orgInfo.name != "" && alreadyOrg === false) {
      const result = await sendOrg(orgInfo.name, orgInfo.description);
      if(result?.status === 200) {
        navigate("/");
      }
    } else {
      const result = await editOrg(orgInfo);
      console.log(result);
    }
  };

  const fetchOrganization = async () => {
    const result = await fetchOrg();
    if (result?.status === 200) {
      setAlreadyOrg(true);
      console.log(result?.data);
      setOrgInfo(result.data);
      console.log(alreadyOrg);
    }
  };

  useEffect(() => {
    fetchOrganization();
  }, []);

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
              <input
                type="email"
                id="emailRegister"
                value={userEmail}
                readOnly
              />

              <span className="icon-inside">
                <i className="bi bi-lock"></i>
              </span>
            </div>
            <label htmlFor="nameRegister">Organisasjons navn</label>
            <input
              type="text"
              id="nameRegister"
              name="name"
              onChange={handleChange}
              value={orgInfo.name}
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
              value={orgInfo.description}
            />
            <div className="imgContainer disclaimerDiv">
              <p>Last opp bilde:</p>
              <button className="chooseBtn">Velg bilde</button>
            </div>
            <div className="btnDiv">
              <button className="canclBtn">Avbryt</button>
              <button className="createBtn" onClick={handleSubmit}>
                {alreadyOrg === false
                  ? "Opprett Organisasjon"
                  : "Endre Organisasjon"}
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
