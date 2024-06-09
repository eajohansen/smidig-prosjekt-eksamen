import React from "react";
import { MainNav } from "../components/shared/MainNav";
import { ProfileForm } from "../components/ProfileForm";
import { useState, useEffect, SyntheticEvent } from "react";
import "bootstrap-icons/font/bootstrap-icons.css";
import { sendUser } from "../services/tempService";
export const ProfilePage = () => {
    return (
        <>
        <MainNav/>
       <div className="UserProfileContainer">
          <div className="headderContainer">
        <h2>Min Profil</h2>
        <hr></hr>
        </div>
    <div className="profileInfoContainer inputHover">
      <div className="userInfoContainer">
        <label htmlFor="emailRegister">Epost</label>
        <div className="emailContainer">
          <input type="email" id="emailRegister" readOnly />
          <span className="icon-inside">
            <i className="bi bi-lock"></i>
          </span>
        </div>
        <label htmlFor="fNameInput">Fornavn</label>
        <input
          type="text"
          id="fNameInput"
          name="fName"
          />
        <label htmlFor="lNameInput">Etternavn</label>
        <input
          type="text"
          id="lNameInput"
          name="lName"
          />
          <a className="chageLogIn chageEM" href="">Endre E-post</a>
           <a className="chageLogIn" href="">Endre passord</a>
      </div>
      <div className="allergyContainer">
          <label className="allergies" htmlFor="allergyInput">
              Legg til allergier
          </label>        
          <div className="allergyBtnDiv">
          <input
            type="text"
            id="allergyInput"
            name="allergier"
            />
          <button className="addBtn">
            Legg til
          </button>
        </div>
        <label className="yourAllergiProfile">Dine allergier</label>
        <div className="allergyOutput">
          <ul>
            {/* {allergies.map((item, i) => (
                <li className="allergy" key={i}>
                {item}
                <i className="trash bi bi-trash3 "></i>
              </li>
            ))} */}
          </ul>
        </div>
        <label className ="moreInfoTxt" htmlFor="moreInfo">Ønsker du å oppgi annen viktig informasjon om deg selv?</label>
        <input type="text" id="moreInfo"/>
        <div className="disclaimerDiv">
          <input className="checkBox" type="checkbox" />
          <label className="disclaimer">
            Ved å registrere brukerprofil, samtykker du til at denne
            informasjonen kan deles med arrangør og eventuelle medarrangører.
          </label>
        </div>
        <div className="btnDiv">
          <button className="canclBtn">Avbryt</button>
          <button className="createBtn">Lagre</button>
        </div>
      </div>
    </div>
  </div>
            </>
    )
    }
