#!/bin/bash

pvc_arr=($(oc get pod | grep Terminating | awk '{print $1}'))

echo ${pvc_arr[1]}

for i in "${pvc_arr[@]}"
do
    kubectl delete pod $i --grace-period=0 --force --namespace d8a8f9-tools
done
