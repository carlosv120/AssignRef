import React, { useEffect, useState } from 'react'
import { useLocation, useParams, useNavigate } from "react-router-dom";
import { ErrorMessage, Field, Form, Formik } from "formik";
import { Card, Row } from "react-bootstrap";
import { CardBody, Col } from "reactstrap";
import debug from "sabio-debug";
import TitleHeader from "components/general/TitleHeader";
import Swal from "sweetalert2";
import toastr from "toastr";
import seasonService from 'services/seasonService';
import testService from 'services/testService';
import certificationService from 'services/certificationService';
import CertificationBasicSchema from 'schemas/certificationFormSchema';

import PropTypes from "prop-types"
import conferencesService from 'services/conferenceService';

function CertificationForm(props) {

    const location = useLocation();
    const navigate = useNavigate();
    const { certificationId } = useParams();
    const _logger = debug.extend("CertificationForm");
    const isAdmin = props.currentUser.roles.some(role => role === "Admin");

    const [formData, setFormData] = useState({
        name: " ",
        seasonId: "",
        isPhysicalRequired: false,
        isBackgroundCheckRequired: false,
        isTestRequired: false,
        testId: "",
        minimumScoreRequired: "",
        isFitnessTestRequired: false,
        isClinicRequired: false,
        dueDate: ""
    });

    const [renderingData, setRenderingData] = useState({
        conferenceId: props.currentUser.conferenceId,
        seasons: "",
        tests: "",
        conferences: "",
    })


    useEffect(() => {

        if (isAdmin) {
            conferencesService.getAll().then(onGetAllConferencesSuccess).catch(onGetAllConferencesError)

            seasonService.getByConferenceId(renderingData.conferenceId).then(onGetSeasonsSuccess).catch(onGetSeasonsError)
            testService.getByConferenceId(renderingData.conferenceId).then(onGetTestsSuccess).catch(onGetTestsError)
        }
        else {
            seasonService.getByConferenceId(renderingData.conferenceId).then(onGetSeasonsSuccess).catch(onGetSeasonsError)
            testService.getByConferenceId(renderingData.conferenceId).then(onGetTestsSuccess).catch(onGetTestsError)
        }
    }, [renderingData.conferenceId])

    useEffect(() => {

        if (certificationId && location.state.type === "CERTIFICATION_EDIT") {

            const aCertification = location.state.payload
            _logger("certification object edit", aCertification)
            setRenderingData(prevState => ({ ...prevState, conferenceId: aCertification.season.conference.id }))
            setFormData((prevState) => {
                const newState = { ...prevState };

                newState.id = certificationId;
                newState.name = aCertification.name;
                newState.seasonId = aCertification.season.id;

                const [date] = aCertification.dueDate.split("T");
                newState.dueDate = date;

                newState.isPhysicalRequired = aCertification.isPhysicalRequired;
                newState.isBackgroundCheckRequired = aCertification.isBackgroundCheckRequired;
                newState.isFitnessTestRequired = aCertification.isFitnessTestRequired;
                newState.isClinicRequired = aCertification.isClinicRequired;
                newState.isTestRequired = aCertification.isTestRequired;

                if (aCertification.isTestRequired) {
                    newState.testId = aCertification.test.id;
                    newState.minimumScoreRequired = aCertification.test.minimumScoreRequired;
                }

                return newState;
            })

        } else {

            setFormData((prevState) => {
                const newState = { ...prevState };

                newState.name = "";
                newState.seasonId = "";
                newState.dueDate = "";
                newState.isPhysicalRequired = false;
                newState.isBackgroundCheckRequired = false;
                newState.isFitnessTestRequired = false;
                newState.isClinicRequired = false;
                newState.isTestRequired = false;
                return newState;
            })
        }

    }, [certificationId])

    const onGetSeasonsSuccess = (data) => {

        if (data.items.length === 0) {
            toastr.warning("No Seasons Found for Conference ", "Warning")
        }

        const seasonsArray = data.items;
        const mappedArray = seasonsArray.map(mappingOptions)
        setRenderingData(prevState => ({ ...prevState, seasons: mappedArray }));
    }

    const onGetTestsSuccess = (data) => {

        const testsArray = data.items;
        _logger("tests", data);

        const mappedArray = testsArray.map(mappingOptions);
        setRenderingData(prevState => ({ ...prevState, tests: mappedArray }));
    }

    const onGetAllConferencesSuccess = (data) => {

        const conferencesArray = data.items;
        const mappedArray = conferencesArray.map(mappingOptions)
        setRenderingData(prevState => ({ ...prevState, conferences: mappedArray }));
    }

    const mappingOptions = (anOption) => {

        return (<option value={anOption.id} key={"option" + anOption.id} > {anOption.name}</option>)
    }

    const onCertificationFormSubmit = (values) => {

        const payload = { ...values };

        if (!payload.isTestRequired) {
            payload.testId = null
            payload.minimumScoreRequired = null
        }

        if (certificationId) {
            certificationService.update(payload).then(onEditCertificationSuccess).catch(onEditCertificationError)
        }
        else {
            certificationService.create(payload).then(onAddCertificationSuccess).catch(onAddCertificationError)
        }
    }

    const onAddCertificationSuccess = (response) => {

        const newId = response.item

        setFormData((previousState) => {
            const newState = { ...previousState, id: newId };

            return newState;
        })

        Swal.fire({
            title: `Certification Added Successfully`,
            html: `<h3>Certification Number: ${newId}</h3>`,
            icon: 'success',
            confirmButtonColor: '#28a745',
            showConfirmButton: true,
        })
    }

    const onEditCertificationSuccess = (response) => {
        _logger("edit success", response)
        Swal.fire({
            title: `Certification Edited Successfully`,
            html: `<h3>Certification Number: ${certificationId}</h3>`,
            icon: 'success',
            iconColor: '#ffa95f',
            confirmButtonColor: '#ffa95f',
            showConfirmButton: true,
            allowOutsideClick: false
        }).then((result) => {
            if (result.isConfirmed) {
                navigate("/certifications")
            }
        })
    }

    const onAddCertificationError = () => {
        toastr.error("Certifications not created", "Error");
    }

    const onEditCertificationError = () => {
        toastr.error("Edition Failed", "Error")
    }

    const onGetSeasonsError = () => {
        toastr.error("No seasons found in the current Conference", "Error")
        setRenderingData(prevState => ({ ...prevState, seasons: "" }));
    }

    const onGetTestsError = () => {
        toastr.warning("No Tests Found for Conference", "Warning")
        setRenderingData(prevState => ({ ...prevState, tests: "" }));
    }

    const onGetAllConferencesError = () => {
        toastr.warning("No conferences found in the current Database", "Warning")
    }

    const selectingConferenceId = (e, resetForm) => {
        const selectedConference = e.target.value
        setRenderingData(prevState => ({ ...prevState, conferenceId: selectedConference }));
        resetForm()
    }


    return (
        <React.Fragment>
            <TitleHeader title="New Certification" buttonText='All Certifications' buttonLink='/certifications' />
            <Row>
                <Col className='col-sm-8'>
                    <Card>
                        <CardBody>
                            <Formik
                                enableReinitialize={true}
                                initialValues={formData}
                                validationSchema={CertificationBasicSchema}
                                onSubmit={onCertificationFormSubmit}
                            >
                                {({ values, handleChange, setFieldValue, resetForm }) => (
                                    <Form>
                                        {isAdmin &&
                                            <>
                                                <Row className='mt-2 mb-4'>
                                                    <div className='col-4 my-auto'>
                                                        <label className="form-label" htmlFor="conferenceId"><h4>Admin</h4></label>
                                                    </div>
                                                    <div className="col-7">
                                                        <select
                                                            className="form-select mb-2 text-dark form-select-sm"
                                                            aria-label="Default select example"
                                                            id="conferenceId"
                                                            name="conferenceId"
                                                            onChange={(e) => selectingConferenceId(e, resetForm)}
                                                            value={renderingData.conferenceId}
                                                        >

                                                            <option value="">Select a Conference</option>
                                                            {renderingData.conferences}
                                                        </select>
                                                    </div>

                                                </Row>
                                            </>}

                                        <Row className='my-2'>
                                            <div className='col-4 my-auto'>
                                                <label className="form-label" htmlFor="seasonId"><h4>Season</h4></label>
                                            </div>
                                            <div className='col-7'>
                                                <Field
                                                    id="seasonId"
                                                    name="seasonId"
                                                    className="form-select mb-2 text-dark form-select-sm"
                                                    component="select"
                                                >
                                                    <option value="">Select a Season</option>
                                                    {renderingData.seasons}
                                                </Field>
                                                <ErrorMessage
                                                    name="seasonId"
                                                    component="small"
                                                    className="text-danger">
                                                </ErrorMessage>
                                            </div>
                                        </Row>

                                        <Row className='my-4'>
                                            <div className='col-4 my-auto'>
                                                <label className="form-label" htmlFor="dueDate"><h4>Due Date</h4></label>
                                            </div>
                                            <div className='col-7'>
                                                <Field
                                                    id="dueDate"
                                                    name="dueDate"
                                                    className="form-control mb-2 form-select-sm"
                                                    type="date"
                                                >
                                                </Field>
                                                <ErrorMessage
                                                    name="dueDate"
                                                    component="small"
                                                    className="text-danger">
                                                </ErrorMessage>
                                            </div>

                                        </Row>

                                        <Row className='my-4'>
                                            <div className='col-4 my-auto'>
                                                <label className="form-label" htmlFor="name"><h4>Name</h4></label>
                                            </div>
                                            <div className='col-7'>
                                                <Field
                                                    id="name"
                                                    name="name"
                                                    className="form-control mb-2 form-control-sm"
                                                    type="text"
                                                    placeholder="Enter the name of the certification"
                                                ></Field>
                                                <ErrorMessage
                                                    name="name"
                                                    component="small"
                                                    className="text-danger">
                                                </ErrorMessage>
                                            </div>
                                        </Row>

                                        <Row className='my-4'>
                                            <div className='col-8 my-auto'>
                                                <label className="form-label " htmlFor="isPhysicalRequired"><h4>Does the certifications Require Physical Something?</h4></label>
                                            </div>
                                            <div className='col-2 text-end my-auto'>
                                                <Field
                                                    id="isPhysicalRequired"
                                                    name="isPhysicalRequired"
                                                    className="form-check-input p-3 mb-2"
                                                    type="checkbox"
                                                >
                                                </Field>
                                            </div>
                                            <div className='col-2 my-auto'>
                                                <h5> {`${values.isPhysicalRequired ? "Yes" : "No"}`}</h5>
                                            </div>
                                        </Row>

                                        <Row className='my-4'>
                                            <div className='col-8'>
                                                <label className="form-label my-auto" htmlFor="isBackgroundCheckRequired"><h4>Does the certifications Require Background Check?</h4></label>
                                            </div>
                                            <div className='col-2 text-end my-auto'>
                                                <Field
                                                    id="isBackgroundCheckRequired"
                                                    name="isBackgroundCheckRequired"
                                                    className="form-check-input p-3 mb-2"
                                                    type="checkbox"
                                                >
                                                </Field>
                                            </div>
                                            <div className='col-2 my-auto'>
                                                <h5> {`${values.isBackgroundCheckRequired ? "Yes" : "No"}`}</h5>
                                            </div>
                                        </Row>

                                        <Row className='my-4'>
                                            <div className='col-8'>
                                                <label className="form-label my-auto" htmlFor="isFitnessTestRequired"><h4>Does the certifications Require Fitness Test?</h4></label>
                                            </div>
                                            <div className='col-2 text-end my-auto'>
                                                <Field
                                                    id="isFitnessTestRequired"
                                                    name="isFitnessTestRequired"
                                                    className="form-check-input p-3 mb-2"
                                                    type="checkbox"
                                                >
                                                </Field>
                                            </div>
                                            <div className='col-2 my-auto'>
                                                <h5> {`${values.isFitnessTestRequired ? "Yes" : "No"}`}</h5>
                                            </div>
                                        </Row>

                                        <Row className='my-4'>
                                            <div className='col-8 mt-2'>
                                                <label className="form-label" htmlFor="isClinicRequired"><h4>Does the certifications Require Clinic?</h4></label>
                                            </div>
                                            <div className='col-2 text-end my-auto'>
                                                <Field
                                                    id="isClinicRequired"
                                                    name="isClinicRequired"
                                                    className="form-check-input p-3 mb-2"
                                                    type="checkbox"
                                                >
                                                </Field>
                                            </div>
                                            <div className='col-2 my-auto'>
                                                <h5> {`${values.isClinicRequired ? "Yes" : "No"}`}</h5>
                                            </div>
                                        </Row>

                                        <Row className='my-4'>
                                            <div className='col-8 mt-2'>
                                                <label className="form-label" htmlFor="isTestRequired"><h4>Does the certifications Require Test?</h4></label>
                                            </div>
                                            <div className='col-2 text-end my-auto'>
                                                <Field
                                                    id="isTestRequired"
                                                    name="isTestRequired"
                                                    className="form-check-input mb-2 p-3"
                                                    type="checkbox"
                                                    onChange={(e) => {
                                                        if (!e.target.checked) {

                                                            setFieldValue("testId", "")
                                                            setFieldValue("minimumScoreRequired", "")
                                                        }
                                                        handleChange(e)
                                                    }}
                                                >
                                                </Field>
                                            </div>
                                            <div className='col-2 my-auto'>
                                                <h5> {`${values.isTestRequired ? "Yes" : "No"}`}</h5>
                                            </div>
                                        </Row>

                                        {values.isTestRequired &&
                                            <>
                                                <Row className='my-4'>
                                                    <div className='col-6 my-auto text-end'>
                                                        <label className="form-label" htmlFor="testId"><h4>Certification Test</h4></label>
                                                    </div>
                                                    <div className='col-5'>
                                                        <Field
                                                            id="testId"
                                                            name="testId"
                                                            className="form-select mb-2 text-dark form-select-sm"
                                                            component="select"
                                                            disabled={!values.isTestRequired}
                                                        >
                                                            <option value="">Select a Test</option>
                                                            {renderingData.tests}
                                                        </Field>
                                                        <ErrorMessage
                                                            name="testId"
                                                            component="small"
                                                            className="text-danger">
                                                        </ErrorMessage>
                                                    </div>
                                                </Row>
                                                <Row className='my-4'>
                                                    <div className='col-6 my-auto text-end'>
                                                        <label className="form-label" htmlFor="minimumScoreRequired"><h4>Minimum Score Required</h4></label>
                                                    </div>
                                                    <div className='col-5'>
                                                        <Field
                                                            id="minimumScoreRequired"
                                                            name="minimumScoreRequired"
                                                            className="form-control mb-2 form-control-sm"
                                                            type="text"
                                                            placeholder="Enter the minimum score"
                                                            disabled={!values.isTestRequired}
                                                        ></Field>
                                                        <ErrorMessage
                                                            name="minimumScoreRequired"
                                                            component="small"
                                                            className="text-danger">
                                                        </ErrorMessage>
                                                    </div>
                                                </Row>
                                            </>
                                        }
                                        <Row className='col-6 mx-auto mt-6 my-4'>
                                            <button type="submit" className="btn btn-primary">Submit Certification</button>
                                        </Row>
                                    </Form>
                                )}
                            </Formik>
                        </CardBody>
                    </Card>
                </Col>
                <Col className='col-sm-4'>
                    <Card>
                        <CardBody>
                            <Row>
                                <h4 className='text-center'>Assign Certifications to Users</h4>
                            </Row>
                            <Row className='my-4'>
                                <div className='col-10 mt-2'>
                                    <label className="form-label" htmlFor="selectAllOficials"><h4>Select all Officials in Conference</h4></label>
                                </div>
                                <div className='col-2 my-auto'>
                                    <input
                                        id="selectAllOficials"
                                        name="selectAllOficials"
                                        className="form-check-input p-3 mb-2"
                                        type="checkbox"
                                    >
                                    </input>
                                </div>
                            </Row>
                            <Row className='col-6 mx-auto mt-6 my-2'>
                                <button type="submit" className="btn btn-primary">Assign</button>
                            </Row>
                        </CardBody>
                    </Card>
                </Col>
            </Row>
        </React.Fragment>
    )
}

CertificationForm.propTypes = {
    currentUser: PropTypes.shape({
        conferenceId: PropTypes.number.isRequired,
        roles: PropTypes.arrayOf(PropTypes.string).isRequired,
        name: PropTypes.string.isRequired,
        isLoggedIn: PropTypes.bool.isRequired,
        id: PropTypes.number.isRequired,
        email: PropTypes.string.isRequired,
        avatarUrl: PropTypes.string.isRequired
    }
    ).isRequired
}

export default CertificationForm