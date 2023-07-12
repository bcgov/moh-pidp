#!/bin/bash

# WARNING: This will remove all pvcs starting with pvc-

pvc_arr=($(oc get pvc | awk '{print $1}' | grep pvc-))

for i in "${pvc_arr[@]}"
do
    kubectl patch pvc $i -p '{"metadata":{"finalizers":null}}'
    oc delete pvc $i
done