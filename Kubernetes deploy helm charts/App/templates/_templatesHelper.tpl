{{ define "common.env.configuration"}}
- name: ApplicationSettings__DetailedError
  value: "true"
- name: username
  valueFrom:
      secretKeyRef:
        name: "mongo-secrets"
        key: MONGO_INITDB_ROOT_USERNAME
- name: password
  valueFrom:
    secretKeyRef:
      name: "mongo-secrets"
      key: MONGO_INITDB_ROOT_PASSWORD
- name: MongoDbSettings__ConnectionString
  value: mongodb://$(username):$(password)@{{ .Values.db.service }}:{{ .Values.db.servicePort }}/?authSource=admin
{{ end }}