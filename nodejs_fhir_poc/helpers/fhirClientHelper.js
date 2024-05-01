const Client = require('fhir-kit-client');
const fhirClient = new Client({
  baseUrl: 'http://localhost:8080/fhir'
});

async function postData(payload) {
    let response = await fhirClient.create({
       ...payload
    });
    return response;
}

async function getByID(payload) {
    let response = await fhirClient.read({
        ...payload
    });
    return response;
}

async function get(type) {
    let response =  await fhirClient.request(type);
    return response;
}

async function patchData(payload) {
    let response = await fhirClient.patch({
        ...payload
    }); 
    return response;
}

async function deleteData(payload) {
    let response = await fhirClient.delete({ 
        ...payload 
    });
    return response;
}

module.exports = {
    postData: postData,
    get: get,
    getByID: getByID,
    patchData: patchData,
    deleteData: deleteData
};