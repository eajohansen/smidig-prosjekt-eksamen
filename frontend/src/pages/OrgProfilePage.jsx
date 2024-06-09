import React from "react";
import { MainNav } from "../components/shared/MainNav";
import { useState, useEffect, SyntheticEvent } from "react";
import "bootstrap-icons/font/bootstrap-icons.css";
import { sendUser } from "../services/tempService";
export const OrgProfilePage = () => {
  
  return(
    <>
      <MainNav/>
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
                  <a className="chageEMOrg" href="">Endre E-post</a>
                  <a href="">Endre passord</a>
            </div>

          <div className="aboutUsContainer">
          <label htmlFor="AboutUs">Om oss</label>
          <input type="text" id="infoAboutUs"/>
          <div className="imgContainer disclaimerDiv">
          <p>Last opp bilde:</p>
          <button>Velg bilde</button>
          </div>
            <div className="btnDiv">
              <button className="canclBtn">Avbryt</button>
              <button className="createBtn" >Lagre</button>
            </div>
          </div>

        </div> 
      </div>
      </>
  )
  
};
