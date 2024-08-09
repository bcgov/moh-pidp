{{- define "chart.common-labels" }}
    app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
    app.kubernetes.io/managed-by: {{ .Release.Service }}
    app.kubernetes.io/instance: {{ .Release.Name }}
    app.kubernetes.io/name: {{ .Values.app_name }}
    app.kubernetes.io/component: database
    app: {{ .Values.app_name }}
    cluster-name: {{ .Values.app_name }}
{{- end }}
