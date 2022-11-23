module.exports = {
  branches: "main",
  plugins: [
    [
      "@semantic-release/commit-analyzer",
      {
        "preset": "conventional-changelog" 
      }
    ],
    [
      "@semantic-release/release-notes-generator", 
     {
       "preset": "conventional-changelog"
     }
    ],
     [
       "@semantic-release/changelog",
       {
         "changelogFile": "CHANGELOG.md"
       }
     ],
//     [
//       "@semantic-release/git",
//       {
//         "assets": ["CHANGELOG.md"]
//       }
//     ]
   ]
}
