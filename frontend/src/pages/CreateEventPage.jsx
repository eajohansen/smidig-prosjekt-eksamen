import React, {useEffect, useState} from "react";
import { sendEvent } from "../services/tempService";
import { useNavigate } from "react-router-dom";
import "../temp.css";
import "../css/CreateEventPage.css";
export const CreateEventPage = () => {
  const navigate = useNavigate();
  const [checkedFree, setCheckedFree] = useState();
  const organization = 1;
  const [start, setStart] = useState();
  const [end, setEnd] = useState();
  const [customFields, setCustomFields] = useState([]);
  const [newCustomField, setNewCustomField] = useState({
    Description: "",
    Value: false,
  });
  const [event, setEvent] = useState({
    organizationId: 1,
    title: "",
    address: "",
    contactP: "",
    description: "",
    capacity: 0,
    ageLimit: 0,
    food: false,
    free: false,
    published: true,
    private: false,
    start: "",
    startTime: "",
    end: "",
    endTime: "",
    customfieldsDesc: "",
    customfieldsValue: false,
    EventCustomFields: []
  });

  useEffect(() => {
    setNewCustomField(newCustomField);
  }, [newCustomField]);

  const handleChange = (e) => {
    const curTargetVal = e?.currentTarget?.value;
    const curTargetName = e?.currentTarget?.name;
    e.persist();
    if (curTargetVal != null) {
      if (["customfieldsDesc", "customfieldsValue"].includes(curTargetName)) {
        let value =
          curTargetName === "customfieldsValue"
            ? e.currentTarget.checked
            : curTargetVal;
        setNewCustomField((prev) => ({
          ...prev,
          [curTargetName === "customfieldsDesc" ? "Description" : "Value"]:
            value,
        }));
      } else {
        setEvent({ ...event, [curTargetName]: curTargetVal });
      }
    }
  };
  const defaultValue = new Date().toISOString().split("T")[0];

  const handleCustomFields = () => {
    const newEventCustomField = {
      Description: newCustomField.Description,
      Value: newCustomField.Value,
    };

    setEvent((prev) => {
      let newEventCustomFields = [...prev.EventCustomFields];
      newEventCustomFields.push({ CustomField: newEventCustomField });

      // Clear the newCustomField state after adding it to the event
      setNewCustomField({
        Description: "",
        Value: false
      });

      return {
        ...prev,
        EventCustomFields: newEventCustomFields,
      };
    });
  };
  const handleSubmit = async () => {
    const result = await sendEvent(event);
    console.log("her kommer result");
    console.log(result);
    if (result?.status === 200) {
      navigate("/events");
    }
  };

  return (
      <div className="page-container">
        <h2>Opprett arrangement</h2>
        <hr></hr>
        <div className="create-event-box">
          <section className="left-box">
            <div>
              <label htmlFor="title">Tittel*</label>
              <input id="title" name="title" onChange={handleChange}></input>
            </div>
            <div>
              <label htmlFor="address">Adresse*</label>
              <input id="address" name="address" onChange={handleChange}></input>
            </div>
            <div>
              <label htmlFor="contactPerson">Kontakt person*</label>
              <input
                  id="contactPerson"
                  name="contactP"
                  onChange={handleChange}
              ></input>
            </div>
            <div>
              <label htmlFor="description">Beskrivelse*</label>
              <input
                  id="description"
                  name="description"
                  onChange={handleChange}
              ></input>
            </div>
            <div className="event-check-box">
              <div>
                <label htmlFor="maxSeating">Antall Plasser*</label>
                <input id="maxSeating" name="capacity" onChange={handleChange} />
              </div>
              <div>
                <label htmlFor="minAge">Aldersgrense*</label>
                <input id="minAge" name="ageLimit" onChange={handleChange} />
              </div>
              <div>
                <label htmlFor="foodService">Matservering*</label>
                <input className="checkBox food" type="checkbox" onChange={handleChange} />
              </div>
              <div>
                <label htmlFor="freeEvent">Gratis arrangement*</label>
                <input className="checkBox" type="checkbox" onChange={handleChange} />
              </div>
            </div>
          </section>
          <section className="right-box">
            <div className="force-width">
              <label htmlFor="startTime">Start dato og kl.*</label>
              <div className="date-time-format">
                <input
                    id="startDate"
                    type="date"
                    name="start"
                    defaultValue={defaultValue}
                    onChange={handleChange}
                ></input>
                <input
                    id="startTime"
                    type="time"
                    max="23:59"
                    name="startTime"
                    defaultValue={"12:00"}
                    onChange={handleChange}
                ></input>
              </div>

              <label htmlFor="endTime">Slutt dato og kl.*</label>
              <div className="date-time-format">
                <input
                    id="endDate"
                    type="date"
                    name="end"
                    defaultValue={defaultValue}
                    onChange={handleChange}
                ></input>
                <input
                    id="endTime"
                    type="time"
                    name="endTime"
                    defaultValue={"12:00"}
                    onChange={handleChange}
                ></input>
              </div>
              <div className="customFieldsContainer">
                <label className="allergies" htmlFor="allergyInput">
                  Egendefinert felt
                </label>
                <div className="custom-fields-input">
                  <input
                      type="text"
                      id="allergyInput"
                      name="customfieldsDesc"
                      onChange={handleChange}
                      value={newCustomField.Description}
                  />
                  <input className="checkBox custom-field-checkbox" type="checkbox" name="customfieldsValue" value={newCustomField.Value} onChange={handleChange} />
                  <button className="addBtn" onClick={handleCustomFields}>
                    Legg til
                  </button>
                </div>
                <h3>Dine egendefinerte felt</h3>
                <div className="allergyOutput">
                  <ul>
                    {event.EventCustomFields.map((item, i) => (
                        <li className="allergy" key={i}>
                          <span>{item.CustomField.Description} {item.CustomField.Value ? "(ja)" : "(nei)"}</span>
                          <i className="trash bi bi-trash3 "></i>
                        </li>
                    ))}
                  </ul>
                </div>
              </div>
                <div id="upLoad">
                    <label htmlFor="">Last opp bilde: </label>
                    <button className="chooseBtn">Last opp bilde</button>
                </div>
            </div>
              <div className="event-page-button-div">
                  <button className="canclBtn">Avbryt</button>
                  <button onClick={handleSubmit} className="createBtn">Lagre og forhåndsvis</button>
              </div>
          </section>
        </div>
      </div>
  );
};
