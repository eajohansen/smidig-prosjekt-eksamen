import { useState, useEffect, SyntheticEvent } from "react";
export const ProfileForm = ({ userEmail }) => {
  const [allergies, setAllergies] = useState([]);
  const [newAllergy, setNewAllergy] = useState("");
  useEffect(() => {
    setNewAllergy(newAllergy);
  }, [newAllergy]);

  const handleChange = (e) => {
    setNewAllergy(e.currentTarget.value);
  };

  const handleClick = () => {
    setAllergies([...allergies, newAllergy]);
    setNewAllergy("");
  };
  return (
    <div className="profileInfoContainer formContainer">
      <div className="userInfoContainer">
        <h2>Opprett Bruker</h2>
        <label htmlFor="emailRegister">Epost</label>
        <input type="email" id="emailRegister" value={userEmail} readOnly />
        <label htmlFor="fNameInput">Fornavn</label>
        <input type="text" id="fNameInput" />
        <label htmlFor="lNameInput">Etternavn</label>
        <input type="text" id="lNameInput" />
        <label htmlFor="dobInput">Fødselsdato</label>
        <input type="date" id="dobInput" />
      </div>
      <div className="allergyContainer">
        <h2>Opprett Bruker</h2>

        <label htmlFor="allergyInput">Allergier</label>
        <div className="allergyBtnDiv">
          <input
            type="text"
            id="allergyInput"
            onChange={handleChange}
            value={newAllergy}
          />
          <button onClick={handleClick}>Legg til</button>
        </div>
        <label>Dine allergier</label>
        <label>Disclaimer...</label>
        <div className="allergyOutput">
          <ul>
            {allergies.map((item, i) => (
              <li key={i}>{item}</li>
            ))}
          </ul>
        </div>
        <div className="btnDiv">
          <button>Avbryt</button>
          <button>Opprett</button>
        </div>
      </div>
    </div>
  );
};
