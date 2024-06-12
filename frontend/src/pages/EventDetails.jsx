import { useState, useEffect } from "react";
import "../css/EventDetails.css";
import { useParams } from "react-router-dom";
import { getEventById, isAdmin } from "../services/tempService";
import dayjs from "dayjs";
import CustomParseFormat from "dayjs/plugin/customParseFormat";

export const EventDetails = () => {
  const { id } = useParams();
  const [event, setEvent] = useState([{}]);
  const [loading, setLoading] = useState(true);
  const [isAdminOrOrg, setIsAdminOrOrg] = useState(false);
  const [dateTime, setDateTime] = useState({
    timeStart: "",
    timeEnd: "",
    dateStart: "",
    dateEnd: "",
  });

  useEffect(() => {
    loadEvent();
  }, []);

  const loadEvent = async () => {
    const result = await getEventById(id);

    const adminResult = await isAdmin();
    if (adminResult.data.admin || adminResult.data.organizer) {
      setIsAdminOrOrg(true);
    }
    console.log("her kommer adminResult");
    console.log(adminResult.data.admin);
    console.log(adminResult.data.organizer);
    setEvent(result);
    if (result != undefined && result != null) {
      setLoading(false);
      splitDateTime(result);
    }
  };

  const splitDateTime = (eventevent) => {
    dayjs.extend(CustomParseFormat);

    setDateTime({
      timeStart: dayjs(eventevent.startTime).format("HH:mm"),
      timeEnd: dayjs(eventevent.endTime).format("HH:mm"),
      dateStart: dayjs(eventevent.startTime).format("DD.MM.YYYY"),
      dateEnd: dayjs(eventevent.endTime).format("DD.MM.YYYY"),
    });
    console.log(dateTime);
  };

  if (loading) {
    return <p>loading....</p>;
  }
  return (
    <div className= {false ? "eventDetailsContainer publisedEvent" : "eventDetailsContainer"}>
      <div className="eventDetailsLeft">
        <img
          src="https://images.unsplash.com/photo-1717684566059-4d16b456c72a?q=80&w=2938&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
          alt=""
        />
        <div className="infoContainer">
          <div className="infoItem">
            <p>Ledige Plasser:</p>
            <p>{event?.availableCapacity}</p>
          </div>
          <div className="infoItem">
            <p>Aldersgrense:</p>
            <p>{event?.ageLimit}</p>
          </div>
          <div className="infoItem">
            <p>Åpent Arrangement: </p>
            <p>{event?.private ? "nei" : "ja"}</p>
          </div>
          <div className="infoItem">
            <p>Organisasjon:</p>
            <p> {event?.organizationName}</p>
          </div>
          <div className="infoItem">
            <p>Kontaktperson:</p>
            <p> {event?.contactPersonName}</p>
          </div>
        </div>
      </div>
      <div className="eventDetailsRight">
        <div className="eventInfo">
          <h1>{event?.title}</h1>
          <div className="dateTimePlace">
            {dateTime?.dateStart === dateTime?.dateEnd ? (
              <>
                <div className="dateTime">
                  <div className="dateItem">
                    <i className="bi bi-calendar3 icon"></i>
                    <p>{dateTime.dateStart}</p>
                  </div>
                  <div className="timeItem">
                    <i className="bi bi-clock icon"></i>
                    <p>{`${dateTime?.timeStart} - ${dateTime?.timeEnd}`}</p>
                  </div>
                </div>
              </>
            ) : (
              <>
                <div className="dateTime">
                  <div>
                    <div className="dateItem">
                      <i className="bi bi-calendar3 icon"></i>
                      <p>{dateTime?.dateStart}</p>
                    </div>
                    <div className="dateItem">
                      <i className="bi bi-calendar3 icon"></i>
                      <p>{dateTime?.dateEnd}</p>
                    </div>
                  </div>
                  <div>
                    <div className="timeItem">
                      <i className="bi bi-clock icon"></i>
                      <p>{`${dateTime?.timeStart}`}</p>
                    </div>
                    <div className="timeItem">
                      <i className="bi bi-clock icon"></i>
                      <p>{`${dateTime?.timeEnd} `}</p>
                    </div>
                  </div>
                </div>
              </>
            )}
            <div className="place">
              <i className="bi bi-geo-alt icon"></i>
              <p>{event?.placeLocation}</p>
            </div>
          </div>
        </div>
        <div className="eventText">
          <p>{event.description}</p>
        </div>
        <div className="btnDiv">
          <button className="buttons">Rediger</button>
          <button className="publishBtn buttons">
            {isAdminOrOrg ? "Publiser" : "Avpubliser"}
          </button>
        </div>
      </div>
    </div>
  );
};


//   "eventId": 1,
//   "title": "Sample Event",
//   "description": "This is a sample description for the event.",
//   "capacity": 30,
//   "ageLimit": 18,
//   "private": false,
//   "published": true,
//   "placeLocation": "there",
//   "placeUrl": "hye",
//   "imageLink": "hey",
//   "imageDescription": "fheyu",
//   "contactPersonName": "tobias",
//   "contactPersonEmail": "tobias@hto.no",
//   "contactPersonNumber": null,
//   "organizationName": "test",
//   "availableCapacity": 27,
//   "startTime": "2024-01-12T21:12:00",
//   "endTime": "2024-02-13T23:50:00",
//   "eventCustomFields": [
//     {
//       "EventCustomField": "Object"
//     },
//     {
//       "EventCustomField": "Object"
//     }
//   ]
// },
// {
//   "more event": "objects"
// }
// ]
