import React, { useState, useEffect, useRef } from "react";
import debug from "sabio-debug";
import { Formik, Form, Field } from "formik";
import lookUpService from "../../services/lookUpService";
import officialsService from "services/officialsService";
import NewLocation from "../locations/NewLocation";
import userService from "services/userService";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import Swal from "sweetalert2";
import "sweetalert2/src/sweetalert2.scss";
import { useNavigate } from "react-router-dom";
const _logger = debug.extend("OfficialsForm");
import { addLocation } from "services/locationService";

function OfficialsForm() {
  const [officialFormData] = useState({
    primaryPositionId: "",
    locationId: "",
    locationTypeId: "",
    lineOne: "",
    lineTwo: "",
    city: "",
    zip: "",
    stateId: 0,
    latitude: 0,
    longitude: 0,
  });

  const [selectsFormData, setSelectsFormData] = useState({
    fieldPositionsData: [],
    fieldPositionsOpt: [],
  });

  const formikRef = useRef();

  useEffect(() => {
    lookUpService
      .lookUp3Col("FieldPositions")
      .then(onlookUp3ColSuccess)
      .catch(onlookUp3ColError);
  }, []);

  const onlookUp3ColSuccess = (response) => {
    const fieldPositionsArray = response.items;

    setSelectsFormData((prevState) => {
      const newState = { ...prevState };
      newState.fieldPositionsData = fieldPositionsArray;
      newState.fieldPositionsOpt = fieldPositionsArray.map(mapOption);
      return newState;
    });
  };

  const onlookUp3ColError = (error) => {
    _logger("onlookUp3ColError", error);
    toast.error("We could not find any position fields");
  };

  const mapOption = (item) => {
    return (
      <option value={item.id} key={item.id}>
        {item.name}
      </option>
    );
  };

  const onLocationSubmit = (values) => {
    _logger("Clicked Here");
    const newValues = {};
    const objProps = [
      "locationTypeId",
      "lineOne",
      "city",
      "zip",
      "stateId",
      "latitude",
      "longitude",
    ];
    objProps.forEach((property) => {
      newValues[property] = values[property];
    });
    newValues.lineTwo = values.lineTwo || "";
    addLocation(newValues)
      .then((res) => onAddLocationSuccess(res, values))
      .catch(onAddLocationError);
  };

  const onAddLocationSuccess = (response, values) => {
    const newValues = {};
    newValues.primaryPositionId = values.primaryPositionId;
    newValues.locationId = response.item;
    onSubmitFormClicked(newValues);
  };

  const onAddLocationError = (response) => {
    toast.error(response, "We could not create the new location");
  };

  const onSubmitFormClicked = (values) => {
    Swal.fire({
      title: "Do you want to create this Official?",
      showDenyButton: true,
      confirmButtonText: "Yes",
      denyButtonText: `No`,
    }).then((result) => {
      if (result.isConfirmed) {
        officialsService
          .create(values)
          .then(onCreateOfficialSuccess)
          .catch(onCreateOfficialError);
      } else {
        Swal.fire("Official was not created");
      }
    });
  };

  const onCreateOfficialSuccess = (response) => {
    _logger("Create official success", response);
    Swal.fire("Official was successfully created.");
    userService
      .deleteRegistrationSettings()
      .then(onDeleteRegistrationSettingsSuccess)
      .catch(onDeleteRegistrationSettingsError);
  };

  const navigate = useNavigate();
  const onDeleteRegistrationSettingsSuccess = () => {
    navigate("/");
  };

  const onDeleteRegistrationSettingsError = (err) => {
    _logger(err);
  };

  const onCreateOfficialError = (error) => {
    _logger("onCreateOfficialError", error);
    toast.error("We could not create the Official");
  };

  return (
    <React.Fragment>
      <div>
        <div className="card">
          <div className="card-header">
            <h1>Official Details</h1>
          </div>
          <div className="card-body">
            <Formik
              onSubmit={onLocationSubmit}
              enableReunitialize={false}
              initialValues={officialFormData}
              innerRef={formikRef}
            >
              <Form>
                <div className="row my-3">
                  <div className="form-group col-5">
                    <label
                      htmlFor="fieldPosition"
                      className="form-label fw-bold"
                    >
                      Field Position
                    </label>
                    <Field
                      as="select"
                      name="primaryPositionId"
                      className="form-select"
                    >
                      <option value="">Select the Field Position</option>;
                      {selectsFormData.fieldPositionsOpt}
                    </Field>
                  </div>
                </div>
                <div className="mt-5">
                  <NewLocation formikRef={formikRef}></NewLocation>
                </div>
                <div className="my-5">
                  <button className="btn btn-primary" type="submit">
                    Submit
                  </button>
                </div>
              </Form>
            </Formik>
          </div>
        </div>
        <ToastContainer
          position="top-right"
          autoClose={5000}
          hideProgressBar={false}
          newestOnTop={false}
          closeOnClick
          rtl={false}
          pauseOnFocusLoss
          draggable
          pauseOnHover
          theme="light"
        />
      </div>
    </React.Fragment>
  );
}
export default OfficialsForm;
