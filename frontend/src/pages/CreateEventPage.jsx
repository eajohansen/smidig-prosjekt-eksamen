import React, {useEffect, useState} from "react";
import { sendEvent } from "../services/tempService";
import "../temp.css";
import "../css/CreateEventPage.css";
export const CreateEventPage = () => {
  const [checkedFree, setCheckedFree] = useState();
  const organization = 1;
  const [start, setStart] = useState();
  const [end, setEnd] = useState();
  const [customFields, setCustomFields] = useState([]);
  const [newCustomField, setNewCustomField] = useState({
    Description: "",
    Value: false
  });  const [event, setEvent] = useState({
    organizationId: 0,
    title: "",
    address: "",
    contactP: "",
    description: "",
    capacity: 0,
    ageLimit: 0,
    food: false,
    free: false,
    published: false,
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
    console.log(e);
    const curTargetVal = e?.currentTarget?.value;
    const curTargetName = e?.currentTarget?.name;
    e.persist();
    if (curTargetVal != null) {
      if (['customfieldsDesc', 'customfieldsValue'].includes(curTargetName)) {
        let value = curTargetName === 'customfieldsValue' ? e.currentTarget.checked : curTargetVal;
        setNewCustomField(prev => ({
          ...prev,
          [curTargetName === 'customfieldsDesc' ? 'Description' : 'Value']: value
        }));
      } else {
        setEvent({ ...event, [curTargetName]: curTargetVal });
      }
    }
  }

  const handleCustomFields = () => {
    const newEventCustomField = {
      "Description": newCustomField.Description,
      "Value": newCustomField.Value
    };

    setEvent(prev => {
      let newEventCustomFields = [...prev.EventCustomFields];
      newEventCustomFields.push({CustomField: newEventCustomField});

      // Clear the newCustomField state after adding it to the event
      setNewCustomField({
        Description: "",
        Value: false
      });

      return {
        ...prev,
        EventCustomFields: newEventCustomFields
      }
    });
  }
  const handleSubmit = async () => {
    await sendEvent(event);
  };

  return (
      <div className="page-container">
        <h2>Opprett arrangement</h2>
        <hr></hr>
        <div className="create-event-box">
          <section className="left-box">
            <div>
              <label htmlFor="title">Tittel</label>
              <input id="title" name="title" onChange={handleChange}></input>
            </div>
            <div>
              <label htmlFor="address">Adresse</label>
              <input id="address" name="address" onChange={handleChange}></input>
            </div>
            <div>
              <label htmlFor="contactPerson">Kontakt Person</label>
              <input
                  id="contactPerson"
                  name="contactP"
                  onChange={handleChange}
              ></input>
            </div>
            <div>
              <label htmlFor="description">Beskrivelse</label>
              <input
                  id="description"
                  name="description"
                  onChange={handleChange}
              ></input>
            </div>
            <div className="event-check-box">
              <div>
                <label htmlFor="maxSeating">Antall Plasser</label>
                <input id="maxSeating" name="capacity" onChange={handleChange} />
              </div>
              <div>
                <label htmlFor="minAge">Aldersgrense</label>
                <input id="minAge" name="ageLimit" onChange={handleChange} />
              </div>
            </div>
          </section>
          <section className="right-box">
            <div className="force-width">
              <label htmlFor="startTime">Start dato og kl.</label>
              <div className="date-time-format">
                <input
                    id="startDate"
                    type="date"
                    name="start"
                    onChange={handleChange}
                ></input>
                <input
                    id="startTime"
                    type="time"
                    max="23:59"
                    name="startTime"
                    onChange={handleChange}
                ></input>
              </div>

              <label htmlFor="endTime">Slutt dato og kl.</label>
              <div className="date-time-format">
                <input
                    id="endDate"
                    type="date"
                    name="end"
                    onChange={handleChange}
                ></input>
                <input
                    id="endTime"
                    type="time"
                    name="endTime"
                    onChange={handleChange}
                ></input>
              </div>
              <div className="allergyContainer">
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
                  <button onClick={handleCustomFields}>
                    Legg til
                  </button>
                </div>
                <label className="yourAllergies">Dine egendefinerte felt</label>
                <div className="allergyOutput">
                  <ul>
                    {event.EventCustomFields.map((item, i) => (
                        <li className="allergy" key={i}>
                          {console.log(item)}
                          <span>{item.CustomField.Description} {item.CustomField.Value ? "(ja)" : "(nei)"}</span>
                          <i className="trash bi bi-trash3 "></i>
                        </li>
                    ))}
                  </ul>
                </div>
              </div>
            </div>
            <div className="event-page-button-div">
              <button>Last opp bilde</button>
              <button>Avbryt</button>
              <button onClick={handleSubmit}>Lagre og forhåndsvis</button>
            </div>
          </section>
        </div>
      </div>
  );
};
