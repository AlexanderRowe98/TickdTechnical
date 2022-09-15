import { useState, useRef } from "react";
import Loader from "./_loader";
import Modal from "./_modal";
import ApiCall from "../data/_fetch";

export default function CsvForm(props) {
    const [selectedFile, setSelectedFile] = useState(null);
    const [fileName, setFileName] = useState("Choose a file");
    const [loader, setLoader] = useState(null);
    const [errorMsg, setErrorMsg] = useState("");
    const [validationError, setValidationError] = useState("");

    const closeError = () => {
        setValidationError("");
        setErrorMsg("");
    }

    const handleChange = (e) => {
        setSelectedFile(e.target.files[0]);
        setFileName(e.target.files[0].name);
    }

    const handleSubmit = (event) => {
        event.preventDefault();
        if (selectedFile) {           
            submitFile(event);
        }
        else {
            setValidationError('Please add a file before submitting');
        }
    }

    const submitFile = () => {
        if (selectedFile.type != "text/csv") {
            props.response(null);
            setSelectedFile(null);
            setValidationError('Please ensure that the file you upload is in the format `.csv');
            setFileName("Choose a file");
        }
        else {
            setLoader(<Loader />);
            var data = new FormData()
            data.append('file', selectedFile)
            ApiCall(data, handleData, handleError);
            setSelectedFile(null);
        }
    }

    const handleData = (data) => {
        props.response(data);
        setLoader(null);
        setFileName("Choose another file");
    }

    const handleError = (error) => {
        setLoader(null);
        setErrorMsg(error);
    }

    return (
        <>
            {loader}            
            <form onSubmit={handleSubmit}>
                <h2>Upload Meter Readings</h2>
                <label htmlFor="file-upload" className="file-upload">
                    {fileName}
                </label>
                <input
                    id="file-upload"
                    className="file-upload"
                    type="file"
                    onChange={(e) => handleChange(e)}                    
                />                               
                <input type="submit" />
            </form>
            {validationError &&
                <Modal title="Error!" copy={validationError} btnText="Close" closeError={closeError}/>
            } 
            {errorMsg &&
                <Modal title="Error!" copy={errorMsg} btnText="Close" closeError={closeError}/>
            }
        </>
    )
}