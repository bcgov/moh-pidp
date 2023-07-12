#!/bin/bash

ist_arr=($(oc get imagestreamtags | grep pr- | awk '{print $1}'))

echo ${ist_arr[1]}

for i in "${ist_arr[@]}"
do
    oc tag -d $i
done