import * as Yup from "yup";

const CertificationBasicSchema = Yup.object().shape({
    name: Yup.string().min(2, "Minimum Length: 2").required("The Name of the Certification is Required"),
    seasonId: Yup.number().required("The Season is Required"),
    isPhysicalRequired: Yup.boolean().required(),
    isBackgroundCheckRequired: Yup.boolean().required(),
    isTestRequired: Yup.boolean().required(),
    testId: Yup.number().when('isTestRequired', {
        is: (isTestRequired) => isTestRequired === true,
        then: () => Yup.number().required("Select the Test")
    }),
    minimumScoreRequired: Yup.number().when('isTestRequired', {
        is: (isTestRequired) => isTestRequired === true,
        then: () => Yup.number().typeError("Must be a number").required("Include the Minimum Score Required"),
    }),
    isFitnessTestRequired: Yup.boolean().required(),
    isClinicRequired: Yup.boolean().required(),
    dueDate: Yup.date().min(new Date(), "Invalid Due Date").required("Include the Due Date")
})

export default CertificationBasicSchema