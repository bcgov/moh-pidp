#!/bin/bash

pvc_arr=($(oc get pvc | grep Terminating | awk '{print $1}'))

echo ${pvc_arr[1]}

for i in "${pvc_arr[@]}"
do
    kubectl patch pvc $i -p '{"metadata":{"finalizers":null}}'
done