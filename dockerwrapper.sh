cd /backend
dotnet run &

cd /frontend
npm start

wait -n
exit $?