import { useState, useRef } from "react";
import Loader from "./_loader";
import Modal from "./_modal";

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
        console.log('submit func');
        if (selectedFile.type != "text/csv") {
            props.response(null);
            setSelectedFile(null);
            setValidationError('Please ensure that the file you upload is in the format `.csv');
            setFileName("Choose a file");
        }
        else {
            console.log('its csv');
            setLoader(<Loader />);
            var data = new FormData()
            data.append('file', selectedFile)
            fetch('/api/meter-reading-uploads', {
                method: 'POST',
                body: data
            }).then((response) => { 
                if (!response.ok) {
                    return response.text().then(text => {handleError(text)})
                }
                else {
                    return response.json()
                }
            }).then((data) => {
                handleData(data)
            }).catch(
                error => console.error(error)
            );

            setSelectedFile(null);
        }
    }

    const handleData = (data) => {
        props.response(data);
        setLoader(null);
        setFileName("Choose another file");
    }

    const handleError = (error) => {
        setErrorMsg(error);
    }

    return (
        <>
            {loader}
            {validationError &&
                <Modal title="Error!" copy={validationError} btnText="Close" closeError={closeError}/>
            } 
            {errorMsg &&
                <Modal title="Error!" copy={errorMsg} btnText="Close" closeError={closeError}/>
            }
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
        </>
    )
}